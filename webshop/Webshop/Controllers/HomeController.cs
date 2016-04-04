using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Webshop.Models;
using Password;
using Webshop.Database;
using Webshop.Enums;
using Webshop.Authorization;
using Webshop.ViewModels;
using PagedList;
using PagedList.Mvc;

namespace Webshop.Controllers
{
    public class HomeController : Controller
    {
        // GET: /Home/
        public ActionResult Shoppingbag()
        {
            ShoppingBag model;
            model = ((Session)this.Session["__MySessionObject"])?.ShoppingBag ?? new ShoppingBag();
            /*
            bovenstaande lijn is een korte notatie voor het volgende.
            if (session != null && session.CartModel != null)
            {
                model = session.CartModel;
            }
            else
            {
                model = new CartModel();
            }
            */
            return View(model);
        }

        public void CheckSession()
        {
            Session session = (Session)this.Session["__MySessionObject"]; //Check of sessie object al bestaat
            if (session == null)
            {
                Session newSession = new Session();
                this.Session["__MySessionObject"] = newSession;
            }
        }

        [HttpPost]
        public ActionResult Shoppingbag(ShoppingBag model)
        {
            ((Session)this.Session["__MySessionObject"]).ShoppingBag = model;

            /*
            bovenstaande lijn is een korte notatie voor het volgende.
            if (session != null && session.CartModel != null)
            {
                model = session.CartModel;
            }
            else
            {
                model = new CartModel();
            }
            */
            return View(model);
        }

        public ActionResult Orders()
        {
            CheckSession();
            if (TempData["ErrorMessage"] != null)
            {
                ViewBag.Error = TempData["ErrorMessage"].ToString();
            }
            Session session = ((Session)this.Session["__MySessionObject"]);
            if (session.User.Role == UserRole.ADMIN)
            {
                using (DatabaseQuery query = new DatabaseQuery())
                {
                    List<Order> orders = query.GetOrders();
                    return View(orders);
                }
            }
            else
            {
                using (DatabaseQuery query = new DatabaseQuery())
                {
                    List<Order> orders = query.GetOrdersByUser(session.User);
                    return View(orders);
                }
            }
        }

        public ActionResult Order(ulong orderId)
        {
            CheckSession();
            Order order = new Order();
            using (DatabaseQuery query = new DatabaseQuery())
            {
                order = query.GetOrder(orderId);
            }
            return View(order);
        }

        public ActionResult CreateOrder()
        {
            CheckSession();
            Session session = ((Session)this.Session["__MySessionObject"]);
            if (session.User != null)
            {
                Order order = new Order();
                order.DTime = DateTime.Now;
                order.OrderLines = session.ShoppingBag.OrderLines;
                order.User = session.User;
                order.Status = OrderStatus.TOBEPAID;
                using (DatabaseQuery query = new DatabaseQuery())
                {
                    query.CreateOrder(order);
                }
                session.ShoppingBag = new ShoppingBag();
                return RedirectToAction("Account", "Home");
            }
            else
            {
                return RedirectToAction("Login", "Home");
            }
        }

        public ActionResult PayOrder(ulong orderId)
        {
            CheckSession();
            PayOrderViewModel model = new PayOrderViewModel();
            using (DatabaseQuery query = new DatabaseQuery())
            {
                List<PaymentOption> paymentOptions = query.GetPaymentOptions();
                model.PaymentOptions = new SelectList(paymentOptions, "id", "name");
                model.Order = query.GetOrder(orderId);
            }
            return View(model);
        }

        public ActionResult OrderStatusChanged(ulong orderId, OrderStatus status)
        {
            try
            {
                using (DatabaseQuery query = new DatabaseQuery())
                {
                    Order order = query.GetOrder(orderId);
                    order.Status = status;
                    query.UpdateOrderStatus(order);
                }
            }
            catch (Exception e)
            {
                TempData["ErrorMessage"] = "Er is iets fout gegaan met het schrijven naar de database: " + e;
            }
            return RedirectToAction("Orders", "Home");
        }


        public ActionResult Login()
        {
            if (TempData["ErrorMessage"] != null)
            {
                ViewBag.Error = TempData["ErrorMessage"].ToString();
            }

            return View(new LoginDataModel());
        }

        public ActionResult Logout()
        {
            HttpContext.Session.Remove("__MySessionObject");
            return RedirectToAction("Index");
        }

        public ActionResult LoginRequest(LoginDataModel user)
        {
            if (ModelState.IsValid)
            {
                using (DatabaseQuery query = new DatabaseQuery())
                {
                    PBKDF2Password password = query.GetPassword(user);
                    //vraag het password van een gebruiker
                    if (password != null)
                    {   //als het password niet leeg is match het password met de Database
                        PasswordMatcher matcher = new PasswordMatcher(password, user.Password, false); //False kijkt of het interne wachtwoord gedisposed moet worden als hij gematched is.
                        if (matcher.IsMatch) //Als een match is
                        {
                            try
                            {//Probeer een Sessie te maken
                                Session session = (Session)this.Session["__MySessionObject"];
                                using (DatabaseQuery userQuery = new DatabaseQuery())
                                {
                                    session.User = userQuery.GetUser(user); //Klant krijgt een sessie
                                    session.LoggedIn = true;
                                    this.Session["__MySessionObject"] = session;
                                    List<Order> orders = userQuery.GetOrdersByUser(session.User);
                                    foreach(Order order in orders)
                                    {
                                        if (order.Status == OrderStatus.TOBEPAID)
                                        {
                                            double days = (DateTime.Now - order.DTime).TotalDays;
                                            if (days > 14)
                                            {
                                                order.Status = OrderStatus.EXPIRED;
                                                userQuery.UpdateOrderStatus(order);
                                            }
                                        }
                                    }
                                }
                                if (((Session)this.Session["__MySessionObject"]).User.Role == UserRole.MANAGER)
                                {
                                    return RedirectToAction("manage"); //Ga naar manager pagina
                                }
                                else if (((Session)this.Session["__MySessionObject"]).User.Role == UserRole.ADMIN)
                                {
                                    return RedirectToAction("admin"); //Ga naar manager pagina
                                }
                                else if(((Session)this.Session["__MySessionObject"]).ShoppingBag.OrderLines.Count > 0)
                                {
                                    return RedirectToAction("Shoppingbag");
                                }
                                return RedirectToAction("Index"); //Ga terug naar de index
                            }
                            catch(Exception e)
                            {
                                TempData["ErrorMessage"] = "Er is iets fout gegaan met het inloggen: " + e;
                            }
                        }
                    }
                    TempData["ErrorMessage"] = "Gebruikersnaam en wachtwoord combinatie zijn onbekend";
                    return RedirectToAction("login", user); //redirect to faillure
                }
            }
            else
            {
                TempData["ErrorMessage"] = "Gebruikersnaam en/of wachtwoord combinatie is fout";
                return RedirectToAction("Login", "Home", user);
            }
        }

        [HttpPost]
        public ActionResult Register(User user)
        {
            user.Prepare();
            if (ModelState.IsValid)
            {
                using (DatabaseQuery query = new DatabaseQuery())
                {
                    query.CreateUser(user);
                }
                return RedirectToAction("Login");
            }
            return View(user);
        }


        public ActionResult Register()
        {
            CheckSession();
            User model = new User();
            return View(model);
        }

        
        public ActionResult AddProductToShoppingbag(ulong productId)
        {
            CheckSession();
            if (TempData["ErrorMessage"] != null)
            {
                ViewBag.Error = TempData["ErrorMessage"].ToString();
            }

            bool amountChanged = false;
            Session ses = ((Session)this.Session["__MySessionObject"]);
            foreach (OrderLine ol in ses.ShoppingBag.OrderLines)
            {
                if (ol.Product.Id == productId)
                {
                    ol.Amount++;
                    amountChanged = true;
                }
            }
            if (!amountChanged)
            {
                using (DatabaseQuery query = new DatabaseQuery())
                {
                    Product product = query.GetProduct(productId);
                    OrderLine ol = new OrderLine();
                    ol.Product = product;
                    ol.Amount = 1;
                    ses.ShoppingBag.OrderLines.Add(ol);
                }
            }
            this.Session["__MySessionObject"] = ses;
            return RedirectToAction("Products", "Home");
        }

        [AuthorizeRoles(UserRole.ADMIN)]
        public ActionResult Product() 
        {
            if (TempData["ErrorMessage"] != null)
            {
                ViewBag.Error = TempData["ErrorMessage"].ToString();
            }
            using (DatabaseQuery query = new DatabaseQuery())
            { 
                return View(query.GetProducts());
            }
        }

        [AuthorizeRoles(UserRole.ADMIN)]
        public ActionResult CustomerOverview()
        {
            if (TempData["ErrorMessage"] != null)
            {
                ViewBag.Error = TempData["ErrorMessage"].ToString();
            }
            using (DatabaseQuery query = new DatabaseQuery())
            {
                return View(query.GetUsers());
            }
        }

        [AuthorizeRoles(UserRole.ADMIN)]
        public ActionResult CategoryOverview()
        {
            if (TempData["ErrorMessage"] != null)
            {
                ViewBag.Error = TempData["ErrorMessage"].ToString();
            }
            using (DatabaseQuery query = new DatabaseQuery())
            {
                return View(query.GetCategories());
            }
        }

        public ActionResult UserDetails()
        {
            using (DatabaseQuery query = new DatabaseQuery())
            {
                return View(((Session)this.Session["__MySessionObject"]).User);
            }
        }

        public ActionResult Products(int? PageNumber, int? CategoryId)
        {
            CheckSession();
            IndexModel model = fillIndexModel();
            if (CategoryId != null && CategoryId >= 0)
            {
                model.Products.RemoveAll(item => item.Category.Id != (ulong)CategoryId);
            }
            int page = (PageNumber ?? 1);
            if (PageNumber < 1)
            {
                page = 1;
            }

            var products = model.Products;
            var onePageOfProducts = products.ToPagedList(page, 8);

            ViewBag.OnePageOfProducts = onePageOfProducts;
            return View(model);
        }


        private IndexModel fillIndexModel()
        {
            IndexModel model = new IndexModel();
            DatabaseQuery db = new DatabaseQuery();
            model.Categories = db.GetCategories();
            model.Products = db.GetProducts();
            return model;
        }

        public ActionResult Index()
        {  //Index returned een ViewModel
            CheckSession();
            return View(fillIndexModel());
        }

        public ActionResult About()
        {
            ViewBag.Active = "about";
            return View(); //About pagina
        }


        [AuthorizeRoles(UserRole.MANAGER)]
        public ActionResult Manage()
        {
            try
            {
                ManagerViewModel viewModel = new ManagerViewModel();
                DatabaseQuery dbq = new DatabaseQuery();
                viewModel.Orders = dbq.GetOrders();
                viewModel.Orderlines = dbq.GetOrderlines();
                viewModel.OrderlinesAsc = dbq.GetOrderlines();

                List<Product> products = dbq.GetProducts();
                foreach (Product p in products)
                {
                    if (viewModel.Orderlines.Any(ol => ol.Product.Id == p.Id))
                    {
                        continue;
                    }
                    OrderLine o = new OrderLine
                    {
                        Product = p,
                        Amount = 0
                    };
                    viewModel.OrderlinesAsc.Add(o);
                }
                viewModel.OrderlinesAsc.Reverse();

                //take first 10 items from list
                viewModel.OrderlinesAsc.Take(10);
                viewModel.Orderlines.Take(10);
                return View(viewModel);
            }
            catch (Exception e)
            {
                ViewBag.Error = "Er is iets fout gegaan met het ophalen van de producten: " + e;
                return View();
            }
            //return RedirectToAction("View", "Home"); //Return naar de Homepage
        }


        [AuthorizeRoles(UserRole.ADMIN)]//Als je een admin bent mag je de pagina bezoeken
        public ActionResult Admin()
        {
            ViewBag.Active = "admin"; //Laat pagina zien
            return View();
        }

        public ActionResult Contact()
        {
            CheckSession();
            ViewBag.Active = "contact";
            return View(); //Contact pagina
        }

        public ActionResult Delivery()
        {
            ViewBag.Active = "delivery";
            return View(); //Laat pagina zien
        }

        [AuthorizeRoles(UserRole.ADMIN)]
        public ActionResult ProductAdded()
        {
            return View(); //laat pagina zien
        }

        public ActionResult Make_Order(ulong? Id) //nullable int
        {
            if (Id != null)
            {

                ulong productId = (ulong)Id;
                using (DatabaseQuery productQuerry = new DatabaseQuery())
                {
                    Product product = productQuerry.GetProduct(productId);
                    if (product != null) //ALs er een product is
                    {
                        Session session = (Session)this.Session["__MySessionObject"];

                        {

                            using (DatabaseQuery purcasheQuery = new DatabaseQuery())
                            { //Maak een purchase SQL query

                            }
                            RedirectToAction("ProductAdded", "Home");
                        }
                    }
                    else //Redirect naar de home anlshet niet lukt
                    {
                        RedirectToAction("Login", "home");
                    }
                }
            }
            return RedirectToAction("Index", "Home"); //als het wel lukt dan redirect je naar home
        }

        public ActionResult News()
        {
            CheckSession();
            ViewBag.Active = "news";
            return View(); //nieuwsPagina
        }

        public ActionResult Preview(ulong productId)
        {
            CheckSession();
            using (DatabaseQuery querry = new DatabaseQuery())
            {
                Product product = querry.GetProduct(productId);
                return View(product);
            }
        }

        [AuthorizeRoles(UserRole.ADMIN)]
        public ActionResult AddProduct()
        {
            ProductViewModel model = FillProductViewModel();
            return View(model); //Geef pagina weer
        }

        private ProductViewModel FillProductViewModel()
        {
            ProductViewModel model = new ProductViewModel();
            DatabaseQuery dbq = new DatabaseQuery();
            List<Category> categories = dbq.GetCategories();
            model.Categories = new SelectList(categories, "id", "name");
            return model;
        }

        [AuthorizeRoles(UserRole.ADMIN)]
        public ActionResult AddCategory()
        {
            Category model = new Category();
            return View(model); //Geef pagina weer
        }

        public ActionResult FAQ()
        {
            CheckSession();
            ViewBag.Active = "FAQ";

            return View(); //Geef pagina weer
        }

        public ActionResult Account()
        {
            CheckSession();
            ViewBag.Active = "Account";

            return View(); //Geef pagina weer
        }

        public ActionResult ClearShoppingbag()
        {
            ((Session)this.Session["__MySessionObject"]).ShoppingBag.OrderLines = new List<OrderLine>();
            return RedirectToAction("products");
        }

        public ActionResult RemoveProductFromShoppingbag(ulong productId)
        {
            ((Session)this.Session["__MySessionObject"]).ShoppingBag.OrderLines.RemoveAll(product => product.Product.Id == (ulong)productId);
            return RedirectToAction("shoppingbag");
        }

        public ActionResult AddAmount(ulong productId)
        {
            foreach(OrderLine ol in ((Session)this.Session["__MySessionObject"]).ShoppingBag.OrderLines)
            {
                if(ol.Product.Id == productId)
                {
                    ol.Amount++;
                }
            }
            return RedirectToAction("shoppingbag");
        }

        public ActionResult SubtractAmount(ulong productId)
        {
            bool amountIsZero = false;
            foreach (OrderLine ol in ((Session)this.Session["__MySessionObject"]).ShoppingBag.OrderLines)
            {
                if (ol.Product.Id == productId)
                {
                    ol.Amount--;
                    if(ol.Amount <= 0)
                    {
                        amountIsZero = true;
                    }
                }
            }
            if (amountIsZero)
            {
                ((Session)this.Session["__MySessionObject"]).ShoppingBag.OrderLines.RemoveAll(product => product.Product.Id == (ulong)productId);
            }
            return RedirectToAction("shoppingbag");
        }

        public ActionResult SearchResult(string Search)
        {
            CheckSession();
            //try {
            SearchResultModel model = new SearchResultModel(Search);
            return View(model);
            //}
            //catch (Exception e)
            //{
            //throw (e); //Test of de searchresult werkt.
            //return RedirectToAction("Error", "Home", e);
            //}
        }

        [AuthorizeRoles(UserRole.ADMIN)]
        public ActionResult ChangeProduct(ulong productId)
        {
            try
            {
                using (DatabaseQuery query = new DatabaseQuery())
                {
                    ProductViewModel pvm = FillProductViewModel();
                    pvm.Product = query.GetProduct(productId);
                    pvm.SelectedCategoryId = pvm.Product.Category.Id;
                    return View(pvm);
                }
            }
            catch (Exception e)
            {
                ViewBag.Error = "Er is iets fout gegaan met het ophalen van het product: " + e;
                return View();
            }
        }

        [HttpPost]
        public ActionResult UpdateProduct(ProductViewModel model)
        {
            try
            {
                using (DatabaseQuery query = new DatabaseQuery())
                {
                    model.Product.Category = query.GetCategory(model.SelectedCategoryId);
                    if (ModelState.IsValid & (model.Product.Category != null)) //check of de modelstate goed is
                    {
                        query.UpdateProduct(model.Product);
                        return RedirectToAction("Product");
                    }
                }
            }
            catch (Exception e)
            {
                ViewBag.Error = "Er is iets fout gegaan met het updaten van het product: " + e;
                return RedirectToAction("ChangeProduct", model.Product.Id); //Ga terug naar de Add_product pagina
            }
            return RedirectToAction("product"); //Ga terug naar de Add_product pagina

        }

        [AuthorizeRoles(UserRole.ADMIN)]
        public ActionResult ChangeCategory(ulong categoryId)
        {
            try
            {
                using (DatabaseQuery query = new DatabaseQuery())
                {
                    Category category = query.GetCategory(categoryId);
                    return View(category);
                }
            }
            catch (Exception e)
            {
                ViewBag.Error = "Er is iets fout gegaan met het ophalen van de category: " + e;
                return View();
            }
        }

        public ActionResult ChangeUser(ulong id)
        {
            try
            {
                using (DatabaseQuery query = new DatabaseQuery())
                {
                    User user = query.GetUser(id);
                    return View(user);
                }
            }
            catch (Exception e)
            {
                ViewBag.Error = "Er is iets fout gegaan met het ophalen van de gebruiker: " + e;
                return View();
            }
        }

        [HttpPost]
        public ActionResult UpdateUser(User user)
        {
            try
            {
                user.Prepare();
                //if (TryUpdateModel(user, null, null, new[] { "Password" })) --> werkt niet
                    using (DatabaseQuery query = new DatabaseQuery())
                    {
                        if (ModelState.IsValid) //is niet goed door password (wordt ofc niet opgehaald + password moet los aangepast kunnen worden)
                        {
                            query.UpdateUser(user);
                            ((Session)this.Session["__MySessionObject"]).User = query.GetUser(user.Id);
                            return RedirectToAction("UserDetails");
                        }
                    }
            }
            catch (Exception e)
            {
                ViewBag.Error = "Er is iets fout gegaan met het updaten van de gebruiker: " + e;
                return RedirectToAction("ChangeUser", user);
            }
            return RedirectToAction("UserDetails");
        }

        [HttpPost]
        public ActionResult UpdateCategory(Category category)
        {
            try
            {
                using (DatabaseQuery query = new DatabaseQuery())
                {
                    if (ModelState.IsValid) //check of de modelstate goed is
                    {
                        query.UpdateCategory(category);
                        return RedirectToAction("CategoryOverview");
                    }
                }
            }
            catch (Exception e)
            {
                ViewBag.Error = "Er is iets fout gegaan met het updaten van de categorie: " + e;
                return RedirectToAction("ChangeCategory", category);
            }
            return RedirectToAction("category"); 
        }

        public ActionResult Error(Exception e)
        { //Return de error pagina
            CheckSession();
            return View();
        }

        [HttpPost]
        public ActionResult NewProduct(ProductViewModel model)
        {
            try
            {
                using (DatabaseQuery query = new DatabaseQuery())
                {
                    model.Product.Category = query.GetCategory(model.SelectedCategoryId);
                    if (ModelState.IsValid & (model.Product.Category != null)) //check of de modelstate goed is
                    {
                        query.CreateProduct(model.Product);
                        return RedirectToAction("product"); //Laat de Admin zien als het goed gaat
                    }
                }
            }
            catch(Exception error)
            { 
                ModelState.AddModelError("Product is niet toegevoegd aan de Database" , error);
                //Adds a model error to the errors collection for the model-state dictionary.
            }
            return RedirectToAction("AddProduct", model); //Ga terug naar de Add_product pagina
        }

        [HttpPost]
        public ActionResult NewCategory(Category category)
        {
            try
            {
                if (ModelState.IsValid) //check of de modelstate goed is
                {
                    using (DatabaseQuery query = new DatabaseQuery())
                    {

                        query.CreateCategory(category);
                        return RedirectToAction("index"); //Laat de Admin zien als het goed gaat
                    }
                }
            }
            catch (Exception error)
            {
                ModelState.AddModelError("Product is niet toegevoegd aan de Database", error);
                //Adds a model error to the errors collection for the model-state dictionary.
            }
            return RedirectToAction("AddCategory", category);
        }

        [AuthorizeRoles(UserRole.ADMIN)]
        public ActionResult DeleteProduct(ulong productId)
        {
            try
            {
                using (DatabaseQuery query = new DatabaseQuery())
                {
                    query.DeleteProduct(productId);
                }
            }
            catch (Exception e)
            {
                TempData["ErrorMessage"] = "Er is iets fout gegaan met het verwijderen van het product: " + e;
            }
            return RedirectToAction("product");
        }

        [AuthorizeRoles(UserRole.ADMIN)]
        public ActionResult DeleteCategory(ulong categoryId)
        {
            try
            {
                using (DatabaseQuery query = new DatabaseQuery())
                {
                    query.DeleteCategory(categoryId);
                }
            }
            catch (Exception e)
            {
                TempData["ErrorMessage"] = "Er is iets fout gegaan met het verwijderen van de categorie: " + e;
            }
            return RedirectToAction("CategoryOverview");
        }
    }
}
