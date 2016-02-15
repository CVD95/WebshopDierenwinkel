using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

using WorkshopASPNETMVC_III_Start.Databasecontrollers;
using WorkshopASPNETMVC_III_Start.Models;
using WorkshopASPNETMVC_III_Start.ViewModels;

namespace WorkshopASPNETMVC_III_Start.Controllers
{
    public class GameController : Controller
    {
        //
        // GET: /Spel/

        private GameDBController gameDBController = new GameDBController();
        private GenreDBController genreDBController = new GenreDBController();

        public ActionResult Index()
        {
            try
            {
                List<Game> games = gameDBController.GetGames();
                return View(games);
            }
            catch(Exception e)
            {
                ViewBag.FoutMelding = "Er is iets fout gegaan: " + e;
                return View();
            }
           
        }

        public ActionResult NieuwGame()
        {
            try
            {
                GameViewModel viewModel = new GameViewModel();

                List<Genre> genres = genreDBController.GetGenres();
                Genre emptyGenre = new Genre();
                emptyGenre.ID = -1;
                emptyGenre.Naam = "";
                genres.Insert(0, emptyGenre);

                viewModel.Genres = new SelectList(genres, "ID", "Naam");
                return View(viewModel);
            }
            catch (Exception e)
            {
                ViewBag.FoutMelding = "Er is iets fout gegaan: " + e;
                return View();
            }
        
        }



        [HttpPost]
        public ActionResult NieuwGame(GameViewModel viewModel)
        {
            try
            {
                viewModel.Game.Genre = genreDBController.GetGenre(viewModel.SelectedGenreID);
                if (ModelState.IsValid)
                {
                    gameDBController.InsertGame(viewModel.Game);
                    return RedirectToAction("Index", "Game");
                }
                else
                {
                   viewModel.Genres = getSelectListGenres();
                   return View(viewModel);
                }
            }
            catch (Exception e)
            {
                ViewBag.FoutMelding = "Er is iets fout gegaan: " + e;
                return View();
            }
        }

        public ActionResult WijzigGame(int gameId)
        {
            try
            {
                //Viewmodel aanmaken
                GameViewModel viewModel = new GameViewModel();
                //Te wijzigen game ophalen
                Game game = gameDBController.getGame(gameId);
             
                //Viewmodel vullen
                viewModel.Game = game;
                viewModel.SelectedGenreID = game.Genre.ID;
                //SelectList ophalen voor genres.
                viewModel.Genres = getSelectListGenres();

                //View retourneren met viewModel
                return View(viewModel);
            }
            catch (Exception e)
            {
                ViewBag.FoutMelding = "Er is iets fout gegaan: " + e;
                return View();
            }

        }

        [HttpPost]
        public ActionResult WijzigGame(GameViewModel viewModel)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    viewModel.Game.Genre = genreDBController.GetGenre(viewModel.SelectedGenreID);
                    gameDBController.UpdateGame(viewModel.Game);
                    return RedirectToAction("Index", "Game");
                }
                else
                {
                    viewModel.Genres = getSelectListGenres();
                    return View(viewModel);
                }
               
            }
            catch (Exception e)
            {
                ViewBag.FoutMelding = "Er is iets fout gegaan: " + e;
                return View();
            }
        }

        public ActionResult VerwijderGame(int gameId)
        {
            try
            {
               gameDBController.DeleteGame(gameId);
               return RedirectToAction("Index", "Game");
            }
            catch (Exception e)
            {
                ViewBag.FoutMelding = "Er is iets fout gegaan: " + e;
                return View();
            }

        }


        private SelectList getSelectListGenres()
        {
            List<Genre> genres = genreDBController.GetGenres();
            Genre emptyGenre = new Genre();
            emptyGenre.ID = -1;
            emptyGenre.Naam = "";
            genres.Insert(0, emptyGenre);

            return new SelectList(genres, "ID", "Naam");
        }
    }
}
