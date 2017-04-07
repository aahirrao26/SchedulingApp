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
    public class ReceptionistController : Controller
    {
        private MedicalSchedulerDBEntities db = new MedicalSchedulerDBEntities();
        private ISearchManager SearchManager;
        IAppointmentManager appointmentManager;

        public ReceptionistController()
        {
            SearchManager = new SearchManager();
            appointmentManager = new AppointmentManager();
        }

        public ReceptionistController(ISearchManager _SearchManager)
        {
            SearchManager = _SearchManager;
        }
        public ReceptionistController(IAppointmentManager _appointmentManager)
        {
            appointmentManager = _appointmentManager ;
        }

        // GET: Receptionist
        public ActionResult Index()
        {
            return View();
        }

        // GET : Receptionist/Search
        public ActionResult Search(string firstName, string lastName)
        {
            if (lastName == "")
                return View(SearchManager.GetPatientList(firstName, lastName));
            else if (firstName == "")
                return View(SearchManager.GetPatientList(firstName, lastName));
            else
                return View(SearchManager.GetPatientList(firstName, lastName));
        }

        public ActionResult MakeAppointment(int patientID)
        {
            ViewBag.DoctorID = new SelectList(appointmentManager.GetDoctorList(), "ID", "FullName");            
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult MakeAppointment([Bind(Include = "ID,Details,DoctorID,PatientID,BookedBy,Date,SlotID")] Appointment appointment)
        {
            if (ModelState.IsValid)
            {
                appointment.BookedBy = Convert.ToInt32(Session["LoggedInUser"]);

                bool isBooked = appointmentManager.BookAppointment(appointment);
                if (isBooked)                
                     TempData["UserSuccessMessage"] = "The appointment has been booked";                               
                else
                    TempData["UserErrorMessage"] = "Sorry! The appointment cannot be booked online at this moment";


                return RedirectToAction("Search");
            }

            ViewBag.DoctorID = new SelectList(appointmentManager.GetDoctorList(), "ID", "FullName");           
            return View(appointment);
        }
    }
}