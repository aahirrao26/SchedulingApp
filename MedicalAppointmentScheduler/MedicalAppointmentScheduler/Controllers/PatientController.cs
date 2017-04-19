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
        /// Creates a view displaying all of the patient's appointments and detailes of those appointments
        /// </summary>
        /// <returns></returns>
        public ActionResult ViewUpcomingAppointment()
        {
            int patientID = Convert.ToInt32(Session["LoggedInUser"]);   //Gets patient ID of user

            Dictionary<int, List<Slots>> times = new Dictionary<int, List<Slots>>();    //Holds the times for each appoinment
            List<Appointment> temp = db.Appointments.Where(u => u.PatientID == patientID).ToList(); //Retrieve all appointments for user

            //Gets the appointment times for each appointment
            foreach (Appointment i in temp)
            {
                times.Add(i.ID, db.Slots.Where(u => u.ID == i.SlotID).ToList());
            }
            ViewBag.slots = times;

            return View(db.Appointments.Where(u => u.PatientID == patientID));

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
