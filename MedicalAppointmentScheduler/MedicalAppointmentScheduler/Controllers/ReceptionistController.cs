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
    [OutputCache(NoStore = true, Duration = 0)]
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
        public ReceptionistController(IAppointmentManager _appointmentManager, ISearchManager _SearchManager)
        {
            appointmentManager = _appointmentManager ;
            SearchManager = _SearchManager;
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


                return RedirectToAction("EditAppointment");
            }

            ViewBag.DoctorID = new SelectList(appointmentManager.GetDoctorList(), "ID", "FullName");           
            return View(appointment);
        }

        public ActionResult GetAvailableSlotsFor(int doctorID, DateTime date) {

            var availableSlots = appointmentManager.GetAvailableSlots(doctorID, date);

            return Json(new { availableSlots }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult EditAppointment()
        {
            List<Appointment> appointmentList = appointmentManager.GetAppointmentList();
            return View(appointmentList);                
        }

        public ActionResult DeleteAppointment(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Appointment appointment = appointmentManager.FindAppointment(id);
            if (appointment == null)
            {
                return HttpNotFound();
            }
            return View(appointment);
        }
     
        [HttpPost, ActionName("DeleteAppointment")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteAppointmentConfirmed(int id)
        {
            appointmentManager.DeleteAppointment(id);
            return RedirectToAction("EditAppointment");
        }

        public ActionResult ViewHistory(int patientID)
        {
            var history = appointmentManager.GetPatientAppointmentHistory(patientID);

            return View(history);
        }
    }
}