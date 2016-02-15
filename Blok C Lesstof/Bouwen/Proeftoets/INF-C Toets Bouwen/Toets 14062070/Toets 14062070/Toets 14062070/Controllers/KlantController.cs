using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Toets.Databasecontrollers;
using Toets.Models;

namespace Toets.Controllers
{
    

    public class KlantController : Controller
    {
        private KlantDBController klantController = new KlantDBController();
        //
        // GET: /Klant/

        public ActionResult Index()
        {
            try
            {
                List<Klant> klanten = klantController.GetKlanten();
                return View(klanten);
            }
            catch (Exception e)
            {
                //Foutmelding tonen
                ViewBag.Foutmelding = "Er is iets fout gegaan:" + e;
                return View();
            }

        }


        public ActionResult NoDatabase()
        {
            try
            {
                Klant batman = new Klant { Naam = "Batman" , beschrijving = "Superheld" , Woonplaats = "Batcave" };
                Klant superman = new Klant { Naam = "Superman", beschrijving = "Superheld", Woonplaats = "Aarde" };
                Klant robin = new Klant { Naam = "Robin", beschrijving = "Superheld", Woonplaats = "Batcave" };

                List<Klant> klanten = new List<Klant>();
                klanten.Add(batman);
                klanten.Add(superman);
                klanten.Add(robin);

                return View(klanten);
            }
            catch (Exception e)
            {
                //Foutmelding tonen
                ViewBag.Foutmelding = "Er is iets fout gegaan:" + e;
                return View();
            }

        }

        public ActionResult KlantInvullen()
        {
            try
            {
               
                return View();
            }
            catch (Exception e)
            {
                //Foutmelding tonen
                ViewBag.Foutmelding = "Er is iets fout gegaan:" + e;
                return View();
            }
        }

        [HttpPost]
        public ActionResult CreateKlantModelBinding(Klant klant)
        {
            string geslacht = Request["geslacht"];

            try
            {
                klantController.InsertKlant(klant);

            }
            catch (Exception e)
            {
                ViewBag.Foutmelding = "Er is iets fout gegaan:" + e;
            }
            return RedirectToAction("Index", "Klant");
        }

        public ActionResult pagina1(int klantid)
        {
            try
            {
                Klant klant = klantController.GetKlant(klantid);
                return View(klant);
            }
            catch (Exception e)
            {
                ViewBag.FoutMelding("Er is iets fout gegaan: " + e);
                return View();
            }
        }
        
    }
}
