using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using MedicalAppointmentScheduler.Core.Data;

namespace MedicalAppointmentScheduler.Controllers
{
    public class ReceptionistController : Controller
    {
        private MedicalSchedulerDBEntities db = new MedicalSchedulerDBEntities();

        // GET: Receptionist
        public ActionResult Index()
        {
            return View();
        }

        // GET : Receptionist/Search
        public ActionResult Search(string firstName, string lastName)
        {
            if (lastName == "")
                return View(db.UserDetails.Where(u => (u.FirstName == firstName || firstName == null) && (u.RoleID == 2)).ToList());
            else if (firstName == "")
                return View(db.UserDetails.Where(u => (u.LastName == lastName || lastName == null) && (u.RoleID == 2)).ToList());
            else
                return View(db.UserDetails.Where(u => (u.FirstName == firstName || firstName == null) && (u.LastName == lastName || lastName == null) && (u.RoleID == 2)).ToList());
        }
    }
}