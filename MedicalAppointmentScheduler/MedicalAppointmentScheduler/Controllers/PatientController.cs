using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MedicalAppointmentScheduler.Core.Data;
using MedicalAppointmentScheduler.Core.Business;

namespace MedicalAppointmentScheduler.Controllers
{
    [Authorize]
    public class PatientController : Controller
    {
        private MedicalSchedulerDBEntities db = new MedicalSchedulerDBEntities();
        IAppointmentManager appointmentManager;

        public PatientController()
        {
            appointmentManager = new AppointmentManager();
        }

        public PatientController(IAppointmentManager _appointmentManager)
        {
            appointmentManager = _appointmentManager;
        }

        /**
         * Creates a view displaying all of the patient's appointments and detailes of those appointments
         */
        public ActionResult Index()
        {
            int patientID = Convert.ToInt32(Session["LoggedInUser"]);   //Gets patient ID of user

            Dictionary<int, List<Slots>> times = new Dictionary<int, List<Slots>>();    //Holds the times for each appoinment
            List<Appointment> temp = db.Appointments.Where(u => u.PatientID == patientID).ToList(); //Retrieve all appointments for user

            //Gets the appointment times for each appointment
            foreach (Appointment i in temp)
            {
                times.Add(i.ID,db.Slots.Where(u => u.ID == i.SlotID).ToList());
            }
            ViewBag.slots = times;

            return View(db.Appointments.Where(u => u.PatientID == patientID));
        }
    }
}