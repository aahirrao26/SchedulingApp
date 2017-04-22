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
using PagedList;

namespace MedicalAppointmentScheduler.Controllers
{
    [Authorize]
    [OutputCache(NoStore = true, Duration = 0)]
    public class DoctorController : Controller
    {
        private ISearchManager SearchManager;
        IAppointmentManager appointmentManager;
        int pageSize = 5;
        int pageIndex = 1;

        public DoctorController()
        {
            SearchManager = new SearchManager();
            appointmentManager = new AppointmentManager();
        }

        public DoctorController(ISearchManager _SearchManager, IAppointmentManager _appointmentManager)
        {
            SearchManager = _SearchManager;
            appointmentManager = _appointmentManager;
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

        /// <summary>
        /// Creates a view displaying all of the doctors's appointments and detailes of those appointmentsViewUpcomingAppointment
        /// </summary>
        /// <returns></returns>
        public ActionResult ViewUpcomingAppointment(int? page)
        {
            pageIndex = page.HasValue ? Convert.ToInt32(page) : 1;
            int doctorID = Convert.ToInt32(Session["LoggedInUser"]);   //Gets doctor ID of user
            List<Appointment> appointmentList = appointmentManager.GetUpcomingAppointmentsForDoctor(doctorID); //Retrieve all appointments for user

            return View(appointmentList.ToPagedList(pageIndex, pageSize));
        }
    }
}