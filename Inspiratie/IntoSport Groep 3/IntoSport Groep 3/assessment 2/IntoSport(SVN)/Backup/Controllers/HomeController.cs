using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using IntoSport.Models;
using IntoSport.DatabaseControllers;
using MySql.Data.MySqlClient;

namespace IntoSport.Controllers
{
    public class HomeController : Controller
    {
        private MODBController moDBController = new MODBController();
        private BeheerderDBController beheerderDBController = new BeheerderDBController();
        private KlantDBController klantDBController = new KlantDBController();
        private ProductenDBController productenDBController = new ProductenDBController();
        private CategorieDBController categorieDBController = new CategorieDBController();
        private AanbiedingDBController aanbiedingDBController = new AanbiedingDBController();
        private SportenDBController sportenDBController = new SportenDBController();
        private BestellingDBController bestellingDBController = new BestellingDBController();
        private MailController mailController = new MailController();
        private WinkelwagenDBController winkelwagenDBController = new WinkelwagenDBController();

        //
        // GET: /Home/
        public const string CartKey = "IntoSportCartId";

        public ActionResult Index()
        {
            if (User.IsInRole("BEHEERDER"))
            {
                return RedirectToAction("Beheerder");
            }
            else if (User.IsInRole("MANAGER"))
            {
                return RedirectToAction("Manager");
            }
            else
            {
                //List<Producten> producten = productenDBController.GetAlleProductenInList();
                ProductSportViewModel viewModel = new ProductSportViewModel();
                //List<Producten> producten = productenDBController.GetAlleProductenInList();
                //List<Sport> sporten = sportenDBController.GetSporten();
                //List<MaatModel> maat = productenDBController.GetMaat();

                //viewModel.maat = new SelectList(maat, "id", "Grootte");
                // Pagination
                // List.skip
                // List.Take
                // Pagina nummer

                //viewModel.sporten = sporten;
                //viewModel.producten = producten;
                return View();
            } 
        }

        public ActionResult Winkelwagen()
        {
            try
            {
                //List<Producten> producten = (List<Producten>)HttpContext.Session["cart"];
                //List<WinkelwagenItem> winkelwagenItems = (List<WinkelwagenItem>)HttpContext.Session["cartItem"];
                List<WinkelwagenItem> winkelwagenItems = new List<WinkelwagenItem>();

                if (Request.Cookies[CartKey] != null)
                {
                    WinkelwagenViewModel viewModel = new WinkelwagenViewModel();
                    winkelwagenItems = winkelwagenDBController.GetProductByGuid(Request.Cookies[CartKey].Value);
                    viewModel.winkelwagenItem = winkelwagenItems;
                    viewModel.totaal = winkelwagenDBController.GetTotal(Request.Cookies[CartKey].Value);
                    if (winkelwagenItems != null)
                    {
                        return View(viewModel);
                    }
                    else
                    {
                        return View();
                    }
                    
                }
                else
                {
                    return View();
                }
                /*
                var q = from x in producten
                        group x by x into g
                        let count = g.Count()
                        orderby count descending
                        select new { Value = g.Key, Count = count };

                foreach (var x in q)
                {
                    WinkelwagenItem winkelwagenItem = new WinkelwagenItem(x.Value, x.Count);
                    winkelwagenItems.Add(winkelwagenItem);
                }

                var viewModel = new WinkelwagenViewModel();
                viewModel.winkelwagenItem = winkelwagenItems;

                return View(viewModel);*/
            }
            catch (Exception e)
            {
                throw e;
            }
            /*
            try
            {
                
                List<Producten> product= (List<Producten>)HttpContext.Session["cart"];
                //List<Winkelwagen> winkelwagen = (List<Winkelwagen>)HttpContext.Session["cart"];
                Winkelwagen winkelwagen = new Winkelwagen(product);

                return View(product);
            }
            catch (Exception e)
            {
                ViewBag.Foutmelding = "Er is iets fout gegaan:" + e;
                return View();
            }
             */
        }
        public ActionResult VerwijderWinkelwagenItem(int productcode, string maat, string kleur)
        {
            try
            {
                winkelwagenDBController.VerwijderProduct(productcode, maat, kleur, Request.Cookies[CartKey].Value);

                return RedirectToAction("Winkelwagen");
            }
            catch (Exception e)
            {
                ViewBag.Foutmelding = "Er is iets fout gegaan:" + e;
                throw e;
            }
        }

        public ActionResult UpdateWinkelwagenItemKwantiteitPlus(int productcode, string maat, string kleur)
        {
            try
            {
                // !ATTENTIE :Check GUID
                List<Action> action = new List<Action>();

                action.Add(() => winkelwagenDBController.UpdateWinkelwagenItemKwantiteitPlus(productcode, maat, kleur, Request.Cookies[CartKey].Value));
                action.Add(() => winkelwagenDBController.VerwijderKwantiteit(productcode, maat, kleur, Request.Cookies[CartKey].Value));

                winkelwagenDBController.executeMethodMetMysqlTrans(action);

                //winkelwagenDBController.UpdateWinkelwagenItemKwantiteitPlus(productcode, Request.Cookies[CartKey].Value);
                //winkelwagenDBController.VerwijderKwantiteit(productcode, Request.Cookies[CartKey].Value);

                return RedirectToAction("Winkelwagen");
            }
            catch (Exception e)
            {
                ViewBag.Foutmelding = "Er is iets fout gegaan:" + e;
                throw e;
            }
        }

        public ActionResult Checkout()
        {
            return View();
        }

        public ActionResult UpdateWinkelwagenItemKwantiteit(int productcode, string maat, string kleur)
        {
            try
            {
                // !ATTENTIE: Check GUID
                List<Action> action = new List<Action>();

                action.Add(() => winkelwagenDBController.UpdateWinkelwagenItemKwantiteit(productcode, maat, kleur, Request.Cookies[CartKey].Value));
                action.Add(() => winkelwagenDBController.VerwijderKwantiteit(productcode, maat, kleur, Request.Cookies[CartKey].Value));

                winkelwagenDBController.executeMethodMetMysqlTrans(action);

                //winkelwagenDBController.UpdateWinkelwagenItemKwantiteit(productcode, Request.Cookies[CartKey].Value);
                //winkelwagenDBController.VerwijderKwantiteit(productcode, Request.Cookies[CartKey].Value);
                return RedirectToAction("Winkelwagen");
            }
            catch (Exception e)
            {
                ViewBag.Foutmelding = "Er is iets fout gegaan:" + e;
                throw e;
            }
        }

        public ActionResult WinkelwagenBestelling()
        {
            // MySqlTransaction trans = null;
            if (User.Identity.IsAuthenticated)
            {
                //trans = conn.BeginTransaction();

                winkelwagenDBController.VerwijderAlleTempWinkelwagenItemPassedDate();

                string email = klantDBController.GetKlantEmail(User.Identity.Name);
                int klantId = klantDBController.GetAlleenKlantId(User.Identity.Name);

                string verzendKeuze = Request["bestelling"];

                if(klantDBController.GetKlantMembership(User.Identity.Name)){
                    if (klantDBController.InsertGoldMembership(klantId)){
                        mailController.MailUser(email, "Gefeliciteerd met uw GoldMembership bij IntoSport!", "U bent eindelijk GoldMember, uw Membership duurt tot het einde van dit kalenderjaar. U krijgt hierbij 5% korting bij elk product met deze membership.");
                    }
                }
                List<WinkelwagenItem> winkelwagenItem = winkelwagenDBController.GetWinkelwagenProducten(Request.Cookies[CartKey].Value);
                for(int i = 0; i < winkelwagenItem.Count; i++){
                    Producten product = winkelwagenItem[i].product;

                    int kwantiteit = winkelwagenItem[i].kwantiteit;
                    string kleur = winkelwagenItem[i].kleur;
                    string maat = winkelwagenItem[i].maat;

                    //winkelwagenDBController.InsertFactuur(product, kwantiteit, klantId, Request.Cookies[CartKey].Value);
                    winkelwagenDBController.InsertFactuurEnBestellingDetail(product, kwantiteit, maat, kleur, klantId, Request.Cookies[CartKey].Value, verzendKeuze);
                }
                List<Action> action = new List<Action>();

                //action.Add(() => mailController.MailUser(email, "Uw bestelling van IntoSport", "Uw bestelling is gemaakt, u kunt uw bestelling inzien op uw profiel op onze website. "));
                action.Add(() => winkelwagenDBController.VerwijderAlleProducten(Request.Cookies[CartKey].Value));

                winkelwagenDBController.executeMethodMetMysqlTrans(action);

                //mailController.MailUser(email, "Uw bestelling van IntoSport", "Uw bestelling is gemaakt, u kunt uw bestelling inzien op uw profiel op onze website. ");
                //winkelwagenDBController.VerwijderAlleProducten(Request.Cookies[CartKey].Value);

                if (Request.Cookies[CartKey] != null)
                {
                    Request.Cookies[CartKey].Expires = DateTime.Now.AddDays(-900);
                    Response.Cookies.Add(Request.Cookies[CartKey]);
                }

                return RedirectToAction("Winkelwagen", "Home");
            }
            else
            {
                return RedirectToAction("Login", "Account");
            }
        }
        
        public ActionResult AddToCart(int id, ProductViewModel model)
        {
            /*
             * Onthouden van een cart heeft verschillende mogelijkheden:
             * 1. Als list opslaan in een cookie met Kwantiteit param ( Privacy, en grootte van cookie )
             * 2. Als list opslaan in een Session met kwantiteit param ( Niet houdbaar voor lange tijd )
             * 3. Als een string opslaan zoals "productcode-kwantiteit|productcode-kwantiteit", en met Regex eruit halen ( Goed, maar onhandig )
             * 4. In de database opslaan met behulp van GUID in cookie. Waarbij GUID producten een einddatum hebben van (~1 maand) en verwijderd worden. ( Veel schrijven / lezen vanuit DB, snel te implementeren )
             * 5. 2 Dimensionaal Array dat productcode en kwantiteit opslaat. ( Ingewikkeld om iets in 2D array toe te voegen / wijzigen / vinden(specifiek) / vervangen)
             */
            if (Request.Cookies[CartKey] == null)
            {
                // Als gebruiker is ingelogt en niet ingelogt, zelfde cartKey.
                Guid tempCartId = Guid.NewGuid();
                /*Response.Cookies[CartKey].Value = tempCartId.ToString();
                Response.Cookies[CartKey].Expires = DateTime.Now.AddDays(31);
                Response.Cookies[CartKey].Domain = null;
                */
                HttpCookie myCookie = new HttpCookie(CartKey);
                myCookie[""] = tempCartId.ToString();
                myCookie.Expires = DateTime.Now.AddDays(31d);
                Response.Cookies.Add(myCookie);
            }

            // Identificeer product
            Producten product = productenDBController.GetProduct(id);

            string kleur = Request["Kleur"];
            string maat = productenDBController.GetMaatGrootte(model.selectedMaatID);
            if (kleur == null || maat == null)
            {
                kleur = "zwart";
                maat = "M";
            }
            // Check of Product al in tempTable is met GUID value
            if (winkelwagenDBController.GetTempProduct(product, maat, kleur, Request.Cookies[CartKey].Value))
            {
                // Increment kwantiteit indien aanwezig
                List<Action> action = new List<Action>();

                action.Add(() => winkelwagenDBController.UpdateWinkelwagenItemKwantiteitPlus(product.productCode, maat,  kleur, Request.Cookies[CartKey].Value));

                winkelwagenDBController.executeMethodMetConn(action);
            }
            else
            {
                // Add nieuw product indien niet aanwezig.
                // ATTENTIE: tempTable met GUID moet zich verwijderen na bepaalde tijd / cookie afloop!(Cookie.Expiration)
                winkelwagenDBController.InsertTempWinkelwagenItem(product, maat, kleur, Request.Cookies[CartKey].Value);
            }
            
            // Opslaan in Temporary Database
            return RedirectToAction("Winkelwagen");

            /*
            if (Session["cart"] != null)
            {
                List<Producten> producten = (List<Producten>)HttpContext.Session["cart"];
                List<WinkelwagenItem> winkelwagenItems = (List<WinkelwagenItem>)HttpContext.Session["cartItem"];
                Producten product = productenDBController.GetProduct(id);
                producten.Add(product);

                Session["cart"] = producten;
                
                //winkelwagenItems.Find(item => item.product  == product)
                if (winkelwagenItems.Any(item => item.product == product))
                {
                    var index = winkelwagenItems.FindIndex(item => item.product == product);
                    winkelwagenItems[index].kwantiteit++;
                }
                else
                {
                    producten.Add(product);
                    WinkelwagenItem winkelwagenItem = new WinkelwagenItem(product, 1);
                    winkelwagenItems.Add(winkelwagenItem);
                }
                Session["cartItem"] = winkelwagenItems;

            }
            else
            {
                List<Producten> producten = new List<Producten>();
                Producten product = productenDBController.GetProduct(id);
                producten.Add(product);
                Session["cart"] = producten;

                List<WinkelwagenItem> winkelwagenItems = new List<WinkelwagenItem>();
                WinkelwagenItem winkelwagenItem = new WinkelwagenItem(product, 1);
                winkelwagenItems.Add(winkelwagenItem);
                Session["cartItem"] = winkelwagenItems;
            }
            return RedirectToAction("winkelwagen");
            /*
            if (Session["cart"] != null)
            {
                List<Producten> producten = (List<Producten>)HttpContext.Session["cart"];

                Producten product = productenDBController.GetProduct(id);
                if (producten.Contains(product))
                {
                    foreach (Producten item in producten)
                    {
                        if (item.Equals(product))
                        {
                            item.productKwantiteit++;
                        }
                    }
                }
                else
                {
                    product.productKwantiteit = 1;
                    producten.Add(product);
                }
                
                Session["cart"] = producten;
            }
            else
            {
                List<Producten> producten = new List<Producten>();
                Producten product = productenDBController.GetProduct(id);
                producten.Add(product);
                Session["cart"] = producten;
            }
            
            return RedirectToAction("winkelwagen");
            */
        }
        

        public ActionResult Aanbieding()
        {
            try
            {
                ProductViewModel viewModel = new ProductViewModel();
                List<Producten> producten = productenDBController.GetProducten();
                List<MaatModel> maat = productenDBController.GetMaat();
                viewModel.productenList = producten;

                viewModel.maat = new SelectList(maat, "id", "Grootte");

                return View(viewModel);
            }
            catch (Exception e)
            {
                ViewBag.Foutmelding = "Er is iets fout gegaan:" + e;
                return View();
            }
        }

        public ActionResult ZoekProduct()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Zoekresultaten()
        {
            try
            {
                string search = Request["Zoekresultaten"];
                string criteria = Request["Categorie"];
                List<Producten> producten = productenDBController.SearchProducten(search, criteria);
                return View(producten);

            }
            catch (Exception e)
            {
                ViewBag.Foutmelding = "Er is iets misgegaan" + e;
                return View();
            }
        }
        public ActionResult ZoekCategorieIndex()
        {
            List<Sport> sporten = sportenDBController.GetSporten();
            return View(sporten);
        }
        public ActionResult ZoekCategorie(int sport)
        {

            try
            {
                LayoutModel model = new LayoutModel();

                List<Sport> sporten = sportenDBController.GetSporten();
                List<Producten> producten = categorieDBController.GetSpecificProducten(sport);
                List<MaatModel> maat = productenDBController.GetMaat();

                model.producten = producten;
                model.sporten = sporten;
                model.maat = new SelectList(maat, "id", "Grootte");

                return View(model);
            }
            catch (Exception e)
            {
                ViewBag.FoutMelding("Er is iets fout gegaan: " + e);

                return View();

            }


        }

        public ActionResult Contact()
        {
            return View();
        }

        public ActionResult OverOns()
        {
            return View();
        }

        [Authorize(Roles = "KLANT, BEHEERDER")]
        public ActionResult WijzigKlant(int klantId)
        {
            try
            {
                Klant klant = klantDBController.GetKlantId(klantId);
                return View(klant);
            }
            catch (Exception e)
            {
                ViewBag.FoutMelding("Er is iets fout gegaan: " + e);
                return View();
            }
        }

        [HttpPost]
        [Authorize(Roles = "KLANT, BEHEERDER")]
        public ActionResult WijzigKlant(Klant klant)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    klantDBController.UpdateKlant(klant);
                    TempData["Wijziging"] = klant.Naam;
                    return RedirectToAction("Index", "Home");
                }
                catch (Exception e)
                {
                    ViewBag.FoutMelding("Er is iets fout gegaan: " + e);
                    return View();
                }
            }
            else
            {
                return View(klant);
            }
        }

        [Authorize(Roles = "BEHEERDER")]
        public ActionResult Beheerder()
        {
            try
            {
                List<Klant> klanten = beheerderDBController.GetKlant();
                List<BestellingOverzicht> bestellingen = beheerderDBController.GetBestelling();
                List<Producten> producten = beheerderDBController.GetProducten();
                List<CategorieModel> categorie = categorieDBController.GetCategorieen();
                List<Aanbieding> aanbieding = aanbiedingDBController.GetAanbiedingen();
                List<Sport> sporten = sportenDBController.GetSporten();

                // Namen ophalen ipv een ID.
                foreach (CategorieModel categories in categorie)
                {

                    Sport sport = sportenDBController.GetSport(categories.sport);
                    Producten product = productenDBController.GetProduct(categories.product);
                    categories.productnaam = product.productNaam;
                    categories.sportnaam = sport.naam;


                }

                foreach (Producten product in producten)
                {
                    if (product.productAanbiedingscode > 0)
                    {
                        Aanbieding ab = aanbiedingDBController.GetAanbieding(product.productAanbiedingscode);
                        product.productAanbiedingsnaam = ab.naam;
                    }
                    else
                    {
                        product.productAanbiedingsnaam = "";
                    }
                }

                foreach (BestellingOverzicht overzicht in bestellingen)
                {

                    Klant klant = beheerderDBController.GetKlant(overzicht.Klant_code);
                    overzicht.klantnaam = klant.Naam;
                }

                // Einde ophalen
                // Begin binden aan ViewModel
                var viewModel = new BeheerderViewModel();
                viewModel.klantOverzicht = klanten;
                viewModel.bestellingOverzicht = bestellingen;
                viewModel.productenOverzicht = producten;
                viewModel.categorieOverzicht = categorie;
                viewModel.aanbiedingOverzicht = aanbieding;
                viewModel.sportoverzicht = sporten;

                // tempdata
                if (TempData["wijziging"] != null)
                {
                    var wijziging = TempData["wijziging"].ToString();
                    ViewBag.Wijziging = wijziging + " is gewijzigt";
                    TempData.Remove("wijziging");
                }

                return View(viewModel);
            }
            catch (Exception e)
            {
                ViewBag.Foutmelding = "Er is iets fout gegaan:" + e;
                return View();
            }
        }

        [Authorize(Roles = "MANAGER")]
        public ActionResult Manager(int Maand = 1, int Jaar = 1)
        {
            try
            {
                ManagerViewModel totaaloverzicht = new ManagerViewModel();
                totaaloverzicht.overzicht1 = moDBController.GetOmzetOverzicht();
                totaaloverzicht.topwinst = moDBController.GetTopWinstOverzicht();
                totaaloverzicht.topverlies = moDBController.GetTopVerliesOverzicht();
                totaaloverzicht.overzichtPerMaand = moDBController.GetOmzetOverzichtPerMaand(Maand, Jaar);

                return View(totaaloverzicht);
            }
            catch (Exception e)
            {
                //Foutmelding tonen
                ViewBag.Foutmelding = "Er is iets fout gegaan:" + e;
                return View();
            }
        }

        public ActionResult OmzetPerMaand(int Maand = 0, int Jaar = 0)
        {
            try
            {
                List<Manager> overzicht = moDBController.GetOmzetOverzichtPerMaand(Maand, Jaar);

                return View(overzicht);

            }
            catch (Exception e)
            {
                //Foutmelding tonen
                ViewBag.Foutmelding = "Er is iets fout gegaan:" + e;
                return View();
            }
        }

        [Authorize(Roles = "KLANT")]
        public ActionResult Klant()
        {
            try
            {
                string usr = User.Identity.Name;

                List<Klant> klantGegevens = klantDBController.GetKlant(usr);
                List<KlantBestellingOverzicht> bestellingOverzicht = klantDBController.GetBestelling(usr);

                var viewModel = new KlantViewModel();
                viewModel.klantOverzicht = klantGegevens;
                viewModel.bestellingOverzicht = bestellingOverzicht;

                return View(viewModel);
            }
            catch (Exception e)
            {
                ViewBag.Foutmelding = "Er is iets fout gegaan:" + e;
                return View();
            }
        }
        ///<summary>
        ///Product begin
        ///</summary>

        [Authorize(Roles = "BEHEERDER")]
        public ActionResult NieuwProduct()
        {
            ProductViewModel viewModel = new ProductViewModel();
            List<Aanbieding> aanbiedingen = aanbiedingDBController.GetAangepastAanbiedingen();

            Aanbieding emptyproduct = new Aanbieding();
            emptyproduct.aanbiedingscode = -1;
            emptyproduct.naam = "Maak een keuze";
            emptyproduct.kortingspercentage = 0;
            aanbiedingen.Insert(0, emptyproduct);

            viewModel.aanbieding = new SelectList(aanbiedingen, "aanbiedingscode", "naam");

            return View(viewModel);
        }

        [HttpPost]
        [Authorize(Roles = "BEHEERDER")]
        public ActionResult NieuwProduct(ProductViewModel model)
        {
            // List<MaatModel> maten = productenDBController.GetMaat();
            try
            {
                model.aanbiedingen = productenDBController.GetAanbieding(model.SelectedaanbiedingID);

                if (ModelState.IsValid)
                {
                    //productenDBController.InsertProduct(model.producten, model.aanbiedingen.aanbiedingscode);
                    productenDBController.InsertProductEnInkoop(model.producten, model.aanbiedingen.aanbiedingscode);

                    TempData["Wijziging"] = model.producten.productNaam;
                    return RedirectToAction("Beheerder", "Home");
                }
                else
                {
                    model.aanbieding = getSelectListAanbieding();
                    return View(model);

                }
            }
            catch (Exception e)
            {
                ViewBag.FoutMelding = "Er is iets fout gegaan: " + e;
                return View();
            }
        }

        [Authorize(Roles = "BEHEERDER")]
        public ActionResult WijzigenProduct(int productcode)
        {
            ProductViewModel model = new ProductViewModel();
            try
            {

                Producten product = productenDBController.GetProduct(productcode);

                model.producten = product;
                model.SelectedaanbiedingID = product.productAanbiedingscode;
                model.aanbieding = getSelectListAanbieding();

                return View(model);
            }
            catch (Exception e)
            {
                ViewBag.FoutMelding("Er is iets fout gegaan: " + e);
                return View();
            }
        }

        [HttpPost]
        [Authorize(Roles = "BEHEERDER")]
        public ActionResult WijzigenProduct(ProductViewModel model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    model.aanbiedingen = aanbiedingDBController.GetAanbieding(model.SelectedaanbiedingID);
                    productenDBController.UpdateProductEnInkoopVoorraad(model.producten, model.aanbiedingen.aanbiedingscode);
                    TempData["Wijziging"] = model.producten.productNaam;
                    return RedirectToAction("Beheerder", "Home");
                }
                catch (Exception e)
                {
                    ViewBag.FoutMelding("Er is iets fout gegaan: " + e);
                    return View();
                }
            }
            else
            {
                model.aanbieding = getSelectListAanbieding();
                return View(model);
            }

        }
        [Authorize(Roles = "BEHEERDER")]
        public ActionResult VerwijderProduct(int productcode)
        {
            try
            {
                productenDBController.VerwijderProduct(productcode);
                TempData["Wijziging"] = productcode;
            }
            catch (Exception e)
            {
                ViewBag.FoutMelding("Er is iets fout gegaan: " + e);
            }
            return RedirectToAction("Beheerder", "Home");
        }

        public ActionResult NewCategorie()
        {
            BeheerderViewModel viewModel = new BeheerderViewModel();
            List<Producten> producten = beheerderDBController.GetProducten();
            Producten emptyproduct = new Producten();
            emptyproduct.productCode = -1;
            emptyproduct.productNaam = "Maak een keuze";
            producten.Insert(0, emptyproduct);
            
            viewModel.product = new SelectList(producten, "productCode", "productNaam");



            List<Sport> sporten = sportenDBController.GetSporten();
            Sport emptysport = new Sport();
            emptysport.sportcode = -1;
            emptysport.naam = "Maak een keuze";
            sporten.Insert(0, emptysport);

            viewModel.sport = new SelectList(sporten, "sportcode", "naam");
            return View(viewModel);
        }

        ///<summary>
        ///Categorie begin
        ///</summary>
        [HttpPost]
        [Authorize(Roles = "BEHEERDER")]
        public ActionResult NewCategorie(BeheerderViewModel model)
        {
            try
                {
                    model.producten = productenDBController.GetProduct(model.SelectedproductID);
                    model.sporten = sportenDBController.GetSport(model.SelectedSportID);
                    
                    if (ModelState.IsValid)
                    {
                    categorieDBController.InsertCategorie(model.categorie, model.producten.productCode, model.sporten.sportcode);
                    TempData["Wijziging"] = model.categorie.type; 
                    }
            else
            {
                
                
                 model.product = getSelectListProduct();
                 model.sport = getSelectListSport();
                   return View(model);
                 }

                }
                catch (Exception e)
                {
                    ViewBag.FoutMelding = "Er is iets fout gegaan: " + e;
                    return View();

                }
                return RedirectToAction("Beheerder", "Home");
        }

        private SelectList getSelectListSport()
        {
            BeheerderViewModel viewModel = new BeheerderViewModel();
            List<Sport> sporten = beheerderDBController.GetSporten();
            Sport emptySport = new Sport();
            emptySport.sportcode = -1;
            emptySport.naam = "";
            sporten.Insert(0, emptySport);

            return new SelectList(sporten, "sportcode", "naam");
        }

        private SelectList getSelectListProduct()
        {
            BeheerderViewModel viewModel = new BeheerderViewModel();
            List<Producten> producten = beheerderDBController.GetProducten();
            Producten emptyproduct = new Producten();
            emptyproduct.productCode = -1;
            emptyproduct.productNaam = "";
            producten.Insert(0, emptyproduct);

            return new SelectList(producten, "productCode", "productNaam");
        }

        private SelectList getSelectListBestellingStatus()
        {
            BestellingOverzicht emptyBestelling = new BestellingOverzicht();
            List<BestellingOverzicht> bestellingen = new List<BestellingOverzicht>();
            emptyBestelling.Status = "Behandeling";
            emptyBestelling.Status = "gereed";
            emptyBestelling.Status = "klaar";
            bestellingen.Insert(0, emptyBestelling);

            return new SelectList(bestellingen, "status");
        }

        private SelectList getSelectListAanbieding()
        {
            ProductViewModel viewModel = new ProductViewModel();
            List<Aanbieding> aanbiedingen = aanbiedingDBController.GetAanbiedingen();
            Aanbieding emptyproduct = new Aanbieding();
            emptyproduct.aanbiedingscode = -1;
            emptyproduct.naam = "Maak een keuze";
            emptyproduct.kortingspercentage = 0;
            aanbiedingen.Insert(0, emptyproduct);
            return new SelectList(aanbiedingen, "aanbiedingscode", "naam");

        }

        private SelectList getSelectListMaat()
        {

            ProductViewModel viewModel = new ProductViewModel();
            List<MaatModel> maten = productenDBController.getMaten();
            MaatModel emptymaat = new MaatModel();
            emptymaat.id = -1;
            emptymaat.maatcategorie = "Maak een keuze";
            maten.Insert(0, emptymaat);


            return new SelectList(maten, "id", "maatcategorie");


        }

        [Authorize(Roles = "BEHEERDER")]
        public ActionResult WijzigCategorie(int categorie_id)
        {
            try
            {

                BeheerderViewModel model = new BeheerderViewModel();
                CategorieModel categorie = categorieDBController.GetCategorie(categorie_id);


                model.categorie = categorie;
                model.SelectedproductID = categorie.product;
                model.SelectedSportID = categorie.sport;
                model.product = getSelectListProduct();
                model.sport = getSelectListSport();

                return View(model);
            }
            catch (Exception e)
            {
                ViewBag.FoutMelding("Er is iets fout gegaan: " + e);
                return View();
            }


        }

        [HttpPost]
        [Authorize(Roles = "BEHEERDER")]
        public ActionResult WijzigCategorie(BeheerderViewModel model)
        {

            try
            {
                model.producten = productenDBController.GetProduct(model.SelectedproductID);
                model.sporten = sportenDBController.GetSport(model.SelectedSportID);
                if (ModelState.IsValid)
                {
                    categorieDBController.UpdateCategorie(model.categorie, model.producten.productCode, model.sporten.sportcode);
                    TempData["Wijziging"] = model.categorie.type;
                }
                else { return View(model); }

            }
            catch (Exception e)
            {
                ViewBag.FoutMelding("Er is iets fout gegaan: " + e);

            }
            return RedirectToAction("Beheerder", "Home");
        }

        [Authorize(Roles = "BEHEERDER")]
        public ActionResult VerwijderCategorie(int categorie_id)
        {
            try
            {
                categorieDBController.DeleteCategorie(categorie_id);
                TempData["Wijziging"] = categorie_id;
            }
            catch (Exception e)
            {
                ViewBag.FoutMelding("Er is iets fout gegaan: " + e);

            }
            return RedirectToAction("Beheerder", "Home");
        }

        ///<summary>
        ///Aanbiedingen begin
        ///</summary>
        ///
        [Authorize(Roles = "BEHEERDER")]
        public ActionResult NewAanbieding()
        {
            return View();
        }

        [HttpPost]
        [Authorize(Roles = "BEHEERDER")]
        public ActionResult NewAanbieding(Aanbieding aanbieding)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    aanbiedingDBController.InsertAanbieding(aanbieding);
                    TempData["Wijziging"] = aanbieding.naam;
                    return RedirectToAction("Beheerder", "Home");
                }
                catch (Exception e)
                {
                    ViewBag.FoutMelding = "Er is iets fout gegaan: " + e;
                    return View();
                }
            }
            else
            {
                aanbiedingDBController.InsertAanbieding(aanbieding);
                return View(aanbieding);
            }
        }

        [Authorize(Roles = "BEHEERDER")]
        public ActionResult WijzigAanbieding(int aanbiedingscode)
        {
            try
            {
                Aanbieding aanbieding = aanbiedingDBController.GetAanbieding(aanbiedingscode);

                return View(aanbieding);
            }
            catch (Exception e)
            {
                ViewBag.FoutMelding("Er is iets fout gegaan: " + e);
                return View();
            }


        }

        [HttpPost]
        [Authorize(Roles = "BEHEERDER")]
        public ActionResult WijzigAanbieding(Aanbieding aanbieding)
        {
            aanbieding.aanbiedingscode = 7;
            if (ModelState.IsValid)
            {
                try
                {

                    aanbiedingDBController.UpdateAanbieding(aanbieding);
                    TempData["Wijziging"] = aanbieding.naam;

                }
                catch (Exception e)
                {
                    ViewBag.FoutMelding("Er is iets fout gegaan: " + e);

                }
                return RedirectToAction("Beheerder", "Home");

            }
            else { return View(aanbieding); }
        }

        [Authorize(Roles = "BEHEERDER")]
        public ActionResult VerwijderAanbieding(int aanbiedingscode)
        {
            try
            {
                aanbiedingDBController.VerwijderAanbieding(aanbiedingscode);
                TempData["Wijziging"] = aanbiedingscode;
            }
            catch (Exception e)
            {
                ViewBag.FoutMelding("Er is iets fout gegaan: " + e);

            }
            return RedirectToAction("Beheerder", "Home");
        }
        /*
        public ActionResult WijzigenBestelling(BestellingOverzicht model)
        {
            try
            {
                model = sportenDBController.GetSport(model.SelectedStatus);

                if (ModelState.IsValid)
                {
                    categorieDBController.InsertCategorie(model.categorie, model.producten.productCode);
                    return RedirectToAction("Beheerder", "Home");
                }
                else
                {
                    model.sport = getSelectListSport();
                    return View(model);
                }

            }
            catch (Exception e)
            {
                ViewBag.FoutMelding = "Er is iets fout gegaan: " + e;
                return View();

            }
        }
        */
        [Authorize(Roles = "BEHEERDER")]
        public ActionResult WijzigenBestelling(int bestellings_code)
        {
            try
            {
                BestellingOverzicht bestelling = bestellingDBController.GetBestelling(bestellings_code);
                return View(bestelling);
            }
            catch (Exception e)
            {
                ViewBag.FoutMelding("Er is iets fout gegaan: " + e);
                return View();
            }
        }

        [HttpPost]
        [Authorize(Roles = "BEHEERDER")]
        public ActionResult WijzigenBestelling(BestellingOverzicht bestelling)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    string email = klantDBController.GetKlantEmail(bestelling.Klant_code);

                    bestellingDBController.UpdateBestelling(bestelling);

                    mailController.MailUser(email, "Wijziging in bestelling status", "Een bestelling status is gewijzigt naar: " + bestelling.Status + ", u kunt dit terug vinden op uw profiel op IntoSport");

                    TempData["Wijziging"] = bestelling.Status;

                    return RedirectToAction("Beheerder", "Home");
                }
                catch (Exception e)
                {
                    ViewBag.FoutMelding("Er is iets fout gegaan: " + e);
                    return View();
                }
            }
            else
            {
                return View(bestelling);
            }
        }

        public ActionResult AnnulerenBestelling(int bestellingscode)
        {
            try
            {
                string email = klantDBController.GetKlantEmailMetBestelling(bestellingscode);

                bestellingDBController.AnnulerenBestelling(bestellingscode);

                mailController.MailUser(email, "Annulering", "Uw bestelling met bestellingscode " + bestellingscode + " is geannuleerd");

                return RedirectToAction("Klant", "Home");
            }
            catch (Exception e)
            {
                ViewBag.FoutMelding("Er is iets fout gegaan: " + e);
                return RedirectToAction("Klant", "Home");
            }
        }
        public ActionResult NieuwSport()
        {
            return View();
        }

        public ActionResult VerwijderBestelling(int bestellingscode)
        {
            try
            {
                beheerderDBController.AnnulerenBestelling(bestellingscode);
                return View();
            }
            catch (Exception e)
            {
                ViewBag.FoutMelding("Er is iets fout gegaan: " + e);
                return View();
            }
        }

        [HttpPost]
        public ActionResult NieuwSport(Sport sport)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    sportenDBController.InsertSport(sport);
                    TempData["Wijziging"] = sport.naam;
                    return RedirectToAction("beheerder", "home");
                }
                catch (Exception e)
                {
                    ViewBag.FoutMelding = "Er is iets fout gegaan: " + e;
                    return View();
                }
            }
            else
            {
                return View(sport);
            }
        }

        public ActionResult WijzigenSport(int sportcode)
        {
            try
            {
                Sport sport = sportenDBController.GetSport(sportcode);
                return View(sport);
            }
            catch (Exception e)
            {
                ViewBag.FoutMelding("Er is iets fout gegaan: " + e);
                return View();
            }
        }

        [HttpPost]
        public ActionResult WijzigenSport(Sport sport)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    sportenDBController.UpdateSport(sport);
                    TempData["Wijziging"] = sport.naam;
                    return RedirectToAction("beheerder", "home");
                }
                catch (Exception e)
                {
                    ViewBag.FoutMelding("Er is iets fout gegaan: " + e);
                    return RedirectToAction("beheerder", "Home");
                }
            }
            else
            {
                return RedirectToAction("beheerder", "home");
            }

        }


        public ActionResult VerwijderSport(int sportcode)
        {
            try
            {
                sportenDBController.VerwijderSport(sportcode);
                TempData["Wijziging"] = sportcode;
            }
            catch (Exception e)
            {
                ViewBag.FoutMelding("Er is iets fout gegaan: " + e);
            }
            return RedirectToAction("beheerder", "home");
        }
    }
}
