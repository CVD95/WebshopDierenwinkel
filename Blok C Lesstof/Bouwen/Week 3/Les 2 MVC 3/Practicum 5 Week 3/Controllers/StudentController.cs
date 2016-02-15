using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

using WorkshopASPNETMVC_III_Start.Databasecontrollers;
using WorkshopASPNETMVC_III_Start.Models;

namespace WorkshopASPNETMVC_III_Start.Controllers
{

    public class StudentController : Controller
    {
        private StudentDBController StudentDBController = new StudentDBController();
        //
        // GET: /Genre/
        public ActionResult Index()
        {
            try
            {
                List<Student> studenten = StudentDBController.GetStudents();
                return View(studenten);
            }
            catch (Exception e)
            {
                //Foutmelding tonen
                ViewBag.Foutmelding = "Er is iets fout gegaan:" + e;
                return View();
            }
        }
    }
}
