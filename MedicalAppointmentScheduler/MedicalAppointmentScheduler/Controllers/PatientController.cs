using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using PagedList;
using MedicalAppointmentScheduler.Core.Data;
using MedicalAppointmentScheduler.Core.Business;

namespace MedicalAppointmentScheduler.Controllers
{
    [Authorize]
    [OutputCache(NoStore = true, Duration = 0)]
    public class PatientController : Controller
    {
        private MedicalSchedulerDBEntities db = new MedicalSchedulerDBEntities();
        IAppointmentManager appointmentManager;
        int pageSize = 5;
        int pageIndex = 1;
        public PatientController()
        {
            appointmentManager = new AppointmentManager();
        }

        public PatientController(IAppointmentManager _appointmentManager)
        {
            appointmentManager = _appointmentManager;
        }

       
        public ActionResult Index()
        {
            return View();
        }


        /// <summary>
        /// Creates a view displaying all of the patient's appointments and detailes of those appointmentsViewUpcomingAppointment
        /// </summary>
        /// <returns></returns>
        public ActionResult ViewUpcomingAppointment()
        {
            int patientID = Convert.ToInt32(Session["LoggedInUser"]);   //Gets patient ID of user
            List<Appointment> temp = appointmentManager.GetUpcomingAppointments(patientID); //Retrieve all appointments for user

            return View(temp);
        }

        public ActionResult ViewAppointmentHistory(int? page)
        {
            pageIndex = page.HasValue ? Convert.ToInt32(page) : 1;
            int patientID = Convert.ToInt32(Session["LoggedInUser"]);
            List<Appointment>  appointmentList = appointmentManager.GetPatientAppointmentHistory(patientID);
            return View(appointmentList.ToPagedList(pageIndex, pageSize));
          
        }

        public ActionResult MakeAppointment()
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
                appointment.PatientID = Convert.ToInt32(Session["LoggedInUser"]);
                bool isBooked = appointmentManager.BookAppointment(appointment);            

                return RedirectToAction("ConfirmAppointmentBooking",new { isBooked = isBooked});
            }

            ViewBag.DoctorID = new SelectList(appointmentManager.GetDoctorList(), "ID", "FullName");
            return View(appointment);
        }

        public ActionResult GetAvailableSlotsFor(int doctorID, DateTime date)
        {

            var availableSlots = appointmentManager.GetAvailableSlots(doctorID, date);

            return Json(new { availableSlots }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult ConfirmAppointmentBooking(bool isBooked)
        {
            ViewData["AppointmentBooked"] = isBooked;
            return View();
        }
    }
}
