using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Security;  // Nodig voor FormsAuthentication
using System.Web.Mvc;
using IntoSport.Models;
using IntoSport.DatabaseControllers;

namespace IntoSport.Controllers
{
    public class AccountController : Controller
    {
        private AuthDBController authDBController = new AuthDBController();
        private RegistratieDBController registratieDBController = new RegistratieDBController();
        private KlantDBController klantDBController = new KlantDBController();

        public ViewResult Login()
        {
            FormsAuthentication.SignOut();
            Session.Abandon();

            return View();
        }

        public ViewResult Registreren()
        {
            FormsAuthentication.SignOut();
            Session.Abandon();

            return View();
        }

        [HttpPost]
        public ActionResult Registreren(RegistrerenModel registrerenViewModel)
        {
            if (ModelState.IsValid)
            {
                if (registratieDBController.checkGebruikerDuplicaat(registrerenViewModel.Gebruikersnaam))
                {
                    registratieDBController.InsertKlant(registrerenViewModel);
                    return RedirectToAction("Login", "Account");
                }
                else
                {
                    ModelState.AddModelError("registratieFout", "Gebruikersnaam bestaat al");
                    return View();
                }
                
            }
            else
            {
                ModelState.AddModelError("registratieFout", "Een of meerdere ingevoerde gegevens voldoen niet aan onze eisen");
                return View();
            }
            /*
            try
            {
                if (ModelState.IsValid)
                {
                    registratieDBController.InsertKlantEnAccount(registrerenViewModel);
                }
                else
                {
                    
                }
            }
            catch (Exception e)
            {
                ViewBag.Foutmelding = "Er is iets fout gegaan:" + e;
            }
            return RedirectToAction("Index", "Home");
             */
        }

        [HttpPost]
        public ActionResult Login(LoginModel viewModel, String returnUrl)
        {

            if (ModelState.IsValid)
            {
                bool auth = authDBController.isAuthorized(viewModel.UserName, viewModel.PassWord);

                if (auth)
                {
                    FormsAuthentication.SetAuthCookie(viewModel.UserName, false);
                    if (returnUrl != null)
                    {
                        return Redirect(returnUrl);
                    }
                    else
                    {
                        return RedirectToAction("Index", "Home");
                    }

                }
                else
                {
                    ModelState.AddModelError("loginfout", "Username en Password zijn incorrect");
                    return View();
                }
            }
            else
            {
                return View();
            }
        }
    }
}
