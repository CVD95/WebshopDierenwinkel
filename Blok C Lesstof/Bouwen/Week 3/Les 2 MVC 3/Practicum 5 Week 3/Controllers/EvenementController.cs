using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WorkshopASPNETMVC_III_Start.Databasecontrollers;
using WorkshopASPNETMVC_III_Start.Models;

namespace WorkshopASPNETMVC_III_Start.Controllers
{
    public class EvenementController : Controller
    {
        
        private EvenementDBController evenementController = new EvenementDBController();
        
        //
        // GET: /Evenement/
        public ActionResult Index()
        {

            try
            {
                List<Evenement> evenementen = evenementController.GetEvenementen();
                return View(evenementen);
            }
            catch (Exception e)
            {
                ViewBag.FoutMelding = "Er is iets fout gegaan: " + e;
                return View();
            }

        }

        public ActionResult NieuwEvenement()
        {
            return View();
        }

        [HttpPost]
        public ActionResult NieuwEvenement(Evenement evenement)
        {
            try
            {
                evenementController.InsertEvenement(evenement);
              
            }
            catch (Exception e)
            {
                ViewBag.FoutMelding = "Er is iets fout gegaan: " + e;
                
            }
            return RedirectToAction("Index", "Evenement");
        }

       
        public ActionResult NieuwEvenementMetValidatie()
        {
            return View();
        }

        [HttpPost]
        public ActionResult NieuwEvenementMetValidatie(Evenement evenement)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    evenementController.InsertEvenement(evenement);
                    return RedirectToAction("Index", "Evenement");
                }
                catch (Exception e)
                {
                    ViewBag.FoutMelding = "Er is iets fout gegaan: " + e;
                    return View();
                }
            }
            else
            {
                return View(evenement);
            }
        }

        public ActionResult WijzigEvenement(int evenementID)
        {
                try
                {
                    Evenement evenement = evenementController.GetEvenement(evenementID);
                    return View(evenement);
                }
                catch (Exception e)
                {
                    ViewBag.FoutMelding("Er is iets fout gegaan: " + e);
                    return View();
                }
           
        }

        public ActionResult WijzigEvenementMetValidatie(int evenementID)
        {
            try
            {
                Evenement evenement = evenementController.GetEvenement(evenementID);
                return View(evenement);
            }
            catch (Exception e)
            {
                ViewBag.FoutMelding("Er is iets fout gegaan: " + e);
                return View();
            }
        }

        [HttpPost]
        public ActionResult WijzigEvenementMetValidatie(Evenement evenement)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    evenementController.UpdateEvenement(evenement);
                    return RedirectToAction("Index", "Evenement");
                }
                catch (Exception e)
                {
                    ViewBag.FoutMelding("Er is iets fout gegaan: " + e);
                    return View();
                }
            }
            else
            {
                return View(evenement);
            }
            
        }

        public ActionResult VerwijderEvenement(int evenementID)
        {
            try
            {
                evenementController.DeleteEvenement(evenementID);
            }
            catch (Exception e)
            {
                ViewBag.FoutMelding("Er is iets fout gegaan: " + e);
            }
            return RedirectToAction("Index", "Evenement");
        }

        [HttpPost]
        public ActionResult WijzigEvenement(Evenement evenement)
        {
            try
            {
                evenementController.UpdateEvenement(evenement);
            }
            catch (Exception e)
            {
                ViewBag.FoutMelding("Er is iets fout gegaan: " + e);
            }
            return RedirectToAction("Index", "Evenement");
        }

    
    }
}
