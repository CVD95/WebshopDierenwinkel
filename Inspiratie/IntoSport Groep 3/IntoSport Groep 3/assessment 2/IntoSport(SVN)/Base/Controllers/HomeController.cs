using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace IntoSport.Controllers
{
    public class HomeController : Controller
    {
        //
        // GET: /Home/

        public ActionResult Index()
        {
            return View();
        }
    
        //
        // GET: /Login/
        public ActionResult Login()
        {
            return View();
        }

        //
        // GET: /Login/
        public ActionResult Beheerder()
        {
            return View();
        }

        //
        // GET: /Registreren/
        public ActionResult Registreren()
        {
            return View();
        }
    }
}
