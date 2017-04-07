using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using MedicalAppointmentScheduler.Core.Data;
using MedicalAppointmentScheduler.Core.Business;

namespace MedicalAppointmentScheduler.Controllers
{
    [Authorize]
    public class DoctorController : Controller
    {
        private ISearchManager SearchManager;

        public DoctorController()
        {
            SearchManager = new SearchManager();
        }

        public DoctorController(ISearchManager _SearchManager)
        {
            SearchManager = _SearchManager;
        }

        // GET: Doctor
        public ActionResult Index()
        {
            return View();
        }

        // GET : Doctor/Search
        public ActionResult Search(string firstName, string lastName)
        {
            if (lastName == "")
                return View(SearchManager.GetPatientList(firstName, lastName));
            else if (firstName == "")
                return View(SearchManager.GetPatientList(firstName, lastName));
            else
                return View(SearchManager.GetPatientList(firstName, lastName));
        }
    }
}