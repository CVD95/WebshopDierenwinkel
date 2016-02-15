using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

using WorkshopASPNETMVC_II_Start.Databasecontrollers;
using WorkshopASPNETMVC_II_Start.Models;

namespace WorkshopASPNETMVC_II_Start.Controllers
{
    public class EvenementController : Controller
    {
        private EvenementDBController EvenementDBController = new EvenementDBController();
        //
        // GET: /Genre/
        public ActionResult Index()
        {
            try
            {
                List<Evenement> evenementen = EvenementDBController.GetEvenementen();
                return View(evenementen);
            }
            catch (Exception e)
            {
                //Foutmelding tonen
                ViewBag.Foutmelding = "Er is iets fout gegaan:" + e;
                return View();
            }
        }

        public ActionResult NewEvenementModelBinding()
        {
            return View();
        }

        [HttpPost]
        public ActionResult CreateEvenementModelBinding(Evenement evenement)
        {
            try
            {
                EvenementDBController.InsertEvenement(evenement);

            }
            catch (Exception e)
            {
                ViewBag.Foutmelding = "Er is iets fout gegaan:" + e;
            }
            return RedirectToAction("Index", "Evenement");
        }

    }
}