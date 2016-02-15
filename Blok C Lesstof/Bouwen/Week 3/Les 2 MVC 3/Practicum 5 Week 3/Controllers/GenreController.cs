using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

using WorkshopASPNETMVC_III_Start.Databasecontrollers;
using WorkshopASPNETMVC_III_Start.Models;

namespace WorkshopASPNETMVC_III_Start.Controllers
{

    public class GenreController : Controller
    {
        private GenreDBController genreDBController = new GenreDBController();
        //
        // GET: /Genre/
        public ActionResult Index()
        {
            try
            {
                List<Genre> genres = genreDBController.GetGenres();
                return View(genres);
            }
            catch (Exception e)
            {
                //Foutmelding tonen
                ViewBag.Foutmelding = "Er is iets fout gegaan:" + e;
                return View();
            }
        }


        public ActionResult NewGenreOldSchool()
        {
            return View();
        }

        public ActionResult NewGenreParameterBinding()
        {
            return View();
        }

        public ActionResult NewGenreModelBinding()
        {
            return View();
        }

        [HttpPost]
        public ActionResult CreateGenreOldSchool()
        {
            String name = Request["name"];
            bool verslavend = Request["verslavend"] == "on";
            Genre genre = new Genre { Naam = name, Verslavend = verslavend };
            try
            {
                genreDBController.InsertGenre(genre);
            }
            catch (Exception e)
            {
                ViewBag.Foutmelding = "Er is iets fout gegaan:" + e;
            }
            
            return RedirectToAction("Index", "Genre");
        }

        [HttpPost]
        public ActionResult CreateGenreParameterBinding(String name, String verslavend)
        {
            bool isVerslavend = verslavend == "on";
            Genre genre = new Genre { Naam = name, Verslavend = isVerslavend };
            try
            {
                genreDBController.InsertGenre(genre);
            }
            catch (Exception e)
            {
                ViewBag.Foutmelding = "Er is iets fout gegaan:" + e;
            }
            return RedirectToAction("Index", "Genre");
        }

        [HttpPost]
        public ActionResult CreateGenreModelBinding(Genre genre)
        {
            try
            {
                genreDBController.InsertGenre(genre);

            }
            catch (Exception e)
            {
                ViewBag.Foutmelding = "Er is iets fout gegaan:" + e;
            }
            return RedirectToAction("Index", "Genre");
        }


        public ActionResult WijzigGenre(int genreId)
        {
            try
            {
                Genre genre = genreDBController.GetGenre(genreId);
                return View(genre);
            }
            catch(Exception e)
            {
                ViewBag.FoutMelding("Er is iets fout gegaan: " + e);
                return View();
            }
        }

        public ActionResult TestWijzig(int genreId)
        {
            try
            {
                Genre genre = genreDBController.GetGenre(genreId);
                return View(genre);
            }
            catch (Exception e)
            {
                ViewBag.FoutMelding("Er is iets fout gegaan: " + e);
                return View();
            }

        }

        public ActionResult VerwijderGenre(int genreId)
        {
            try
            {
                genreDBController.DeleteGenre(genreId);
                
            }
            catch(Exception e)
            {
                ViewBag.FoutMelding("Er is iets fout gegaan: " + e);
               
            }
            return RedirectToAction("Index", "Genre");
        }

        [HttpPost]
        public ActionResult WijzigGenre(Genre genre)
        {
            try
            {
                genreDBController.UpdateGenre(genre);
               
            }
            catch (Exception e)
            {
                ViewBag.FoutMelding("Er is iets fout gegaan: " + e);
               
            }
            return RedirectToAction("Index", "Genre");


        }

    }
      
}
