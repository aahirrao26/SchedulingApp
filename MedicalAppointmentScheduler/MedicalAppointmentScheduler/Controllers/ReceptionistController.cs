using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using MedicalAppointmentScheduler.Core.Data;
using MedicalAppointmentScheduler.Core.Business;

namespace MedicalAppointmentScheduler.Controllers
{
    [Authorize]
    public class ReceptionistController : Controller
    {
        private MedicalSchedulerDBEntities db = new MedicalSchedulerDBEntities();
        IAppointmentManager appointmentManager;

        public ReceptionistController()
        {
            appointmentManager = new AppointmentManager();
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
                return View(db.UserDetails.Where(u => (u.FirstName == firstName || firstName == null) && (u.RoleID == 2)).ToList());
            else if (firstName == "")
                return View(db.UserDetails.Where(u => (u.LastName == lastName || lastName == null) && (u.RoleID == 2)).ToList());
            else
                return View(db.UserDetails.Where(u => (u.FirstName == firstName || firstName == null) && (u.LastName == lastName || lastName == null) && (u.RoleID == 2)).ToList());
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

        public ActionResult GetAvailableSlotsFor(int doctorID, DateTime date) {

            var availableSlots = appointmentManager.GetAvailableSlots(doctorID, date);

            return Json(new { availableSlots }, JsonRequestBehavior.AllowGet);
        }
    }
}