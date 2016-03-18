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
        //private ProductDBController productenDBController = new ProductenDBController();

        //
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
            Session session = ((Session)this.Session["__MySessionObject"]);
            if (session.User.Role == UserRole.MANAGER)
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

        public ActionResult CreateOrder()
        {
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

        public ActionResult PayOrder(Order order)
        {
            PayOrderViewModel model = new PayOrderViewModel();
            model.Order = order;
            using (DatabaseQuery query = new DatabaseQuery())
            {
                List<PaymentOption> paymentOptions = query.GetPaymentOptions();
                model.PaymentOptions = new SelectList(paymentOptions, "id", "name");
            }
            return View(model);
        }

        public ActionResult OrderPaid(Order order)
        {
            order.Status = OrderStatus.PAID;
            UpdateOrderStatus(order);
            return View(order);
        }

        public ActionResult OrderProcessing(Order order)
        {
            order.Status = OrderStatus.PROCESSING;
            UpdateOrderStatus(order);
            return View(order);
        }

        public ActionResult OrderExpired(Order order)
        {
            order.Status = OrderStatus.EXPIRED;
            UpdateOrderStatus(order);
            return View(order);
        }

        public ActionResult OrderSent(Order order)
        {
            order.Status = OrderStatus.SENT;
            UpdateOrderStatus(order);
            return View(order);
        }

        public ActionResult OrderReturned(Order order)
        {
            order.Status = OrderStatus.RETURNED;
            UpdateOrderStatus(order);
            return View(order);
        }

        private void UpdateOrderStatus(Order order)
        {
            try
            {
                using (DatabaseQuery query = new DatabaseQuery())
                {
                    query.UpdateOrderStatus(order);
                }
            }
            catch (Exception e)
            {
                ViewBag.Error = "Er is iets fout gegaan met het ophalen van het product: " + e;
            }
        }

        public ActionResult Login(LoginDataModel model)
        {
            if(model == null || !ModelState.IsValid)
            { //first attempt, or something went wrong. assume no username typed.
                model = new LoginDataModel();
            }
            else
            { //second attempt, probably wrong credentials. leave username, but erase pwrd.
                model.Password = null;
                ModelState.AddModelError("Password", "Wachtwoord en / of gebruikersnaam is incorrect.");
            }
            return View(model);
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
                                    return RedirectToAction("CreateOrder");
                                }
                                return RedirectToAction("Index"); //Ga terug naar de index
                            }
                            catch(Exception e)
                            {
                                TempData["ErrorMessage"] = "Er is iets fout gegaan met het inloggen: " + e;
                            }
                        }
                    }
                    return RedirectToAction("login", user); //redirect to faillure
                }
            }
            else
            {
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
            User model = new User();
            return View(model);
        }

        
        public ActionResult AddProductToShoppingbag(ulong productId)
        {
            bool productAdded = false;
            using (DatabaseQuery query = new DatabaseQuery())
            {
                if (TempData["ErrorMessage"] != null)
                {
                    ViewBag.Error = TempData["ErrorMessage"].ToString();
                }
                Session ses = ((Session)this.Session["__MySessionObject"]);
                if (ses.ShoppingBag.OrderLines != null)
                {
                    foreach (OrderLine ol in ses.ShoppingBag.OrderLines)
                    {
                        if (ol.Product.Id == productId)
                        {
                            ol.Amount++;
                            productAdded = true;
                        }
                    }
                }
                if (!productAdded)
                {
                    Product product = query.GetProduct(productId);
                    OrderLine ol = new OrderLine();
                    ol.Product = product;
                    ol.Amount = 1;
                    ses.ShoppingBag.OrderLines.Add(ol);
                }
                this.Session["__MySessionObject"] = ses;
            }
            return RedirectToAction("Products", "Home");
        }


        public ActionResult Product() //Nullable ulong
        {
            using (DatabaseQuery query = new DatabaseQuery())
            { //Als het ID niet null is Krijg alle producten uit de Query
                if (TempData["ErrorMessage"] != null)
                {
                    ViewBag.Error = TempData["ErrorMessage"].ToString();
                }
                return View(query.GetProducts());
            }

        }


        public ActionResult CategoryOverview() //Nullable ulong
        {
            using (DatabaseQuery query = new DatabaseQuery())
            { //Als het ID niet null is Krijg alle producten uit de Query
                if (TempData["ErrorMessage"] != null)
                {
                    ViewBag.Error = TempData["ErrorMessage"].ToString();
                }
                return View(query.GetCategories());
            }

        }

        public ActionResult Customer() //Nullable ulong
        {
            using (DatabaseQuery query = new DatabaseQuery())
            { //Als het ID niet null is Krijg alle producten uit de Query

                return View(query.GetUserDetails());
            }

        }

        
		public ActionResult Products(ProductPageModel p)
		{
            IndexModel model = fillIndexModel();
            model.ProductPageModel = p;
            if (model.ProductPageModel.Category != null)
            {
                for (int i = model.Products.Count - 1; i >= 0; i--)
                {
                    if (!model.Products[i].Category.Name.Equals(ViewBag.Category.Name))
                    {
                        model.Products.RemoveAt(i);
                    }
                }
            }
            int page;
            if(p.PageNumber < 1)
            {
                page = 1;
            }
            else
            {
                page = p.PageNumber;
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
            ViewBag.Active = "index";
            Session session = (Session)this.Session["__MySessionObject"]; //Check of sessie object al bestaat
            if (session == null)
            {
                Session newSession = new Session();
                this.Session["__MySessionObject"] = newSession;
            }
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
            ViewBag.Active = "contact";
            return View(); //Contact pagina
        }

        [Authorize] //Je moet ingelogd zijn om de pagina te mogen bezoeken.
        public ActionResult Delivery()
        {
            ViewBag.Active = "delivery";
            return View(); //Laat pagina zien
        }

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
            ViewBag.Active = "news";
            return View(); //nieuwsPagina
        }

        public ActionResult PreviewFromId(ulong id)
        {
            
            using (DatabaseQuery querry = new DatabaseQuery())
            {
                Product product = querry.GetProduct(id);
                //return RedirectToAction("Preview", "Home", product);
                return View("Preview", product);
            }
        }

        public ActionResult Preview(Product product)
        {
            //here product.Category is null. WHY???
            if (ModelState.IsValid)
            {   //Preview pagina. Laat producten zien
                ViewBag.Active = "preview";
                return View(product);
            }
            return RedirectToAction("Index", "Home");
        }

        [AuthorizeRoles(UserRole.ADMIN)]
        public ActionResult Add_Product()
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
        public ActionResult Add_Category()
        {
            Category model = new Category();
            return View(model); //Geef pagina weer
        }

        public ActionResult FAQ()
        {
            ViewBag.Active = "FAQ";

            return View(); //Geef pagina weer
        }

        public ActionResult Account()
        {
            ViewBag.Active = "Account";

            return View(); //Geef pagina weer
        }

        public ActionResult Search_Result(string Search)
        {
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
        public ActionResult Change_Product(ulong productId)
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
                return RedirectToAction("Change_Product", model.Product.Id); //Ga terug naar de Add_product pagina
            }
            return RedirectToAction("product"); //Ga terug naar de Add_product pagina

        }

        [AuthorizeRoles(UserRole.ADMIN)]
        public ActionResult Change_Category(ulong categoryId)
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
                return RedirectToAction("Change_Category", category);
            }
            return RedirectToAction("category"); 
        }

        public ActionResult Error(Exception e)
        { //Return de error pagina
            return View();
        }

        [HttpPost]
        public ActionResult New_Product(ProductViewModel model)
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
            return RedirectToAction("Add_Product", model); //Ga terug naar de Add_product pagina
        }

        [HttpPost]
        public ActionResult New_Category(Category category)
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
            return RedirectToAction("Add_Category", category);
        }

     
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
