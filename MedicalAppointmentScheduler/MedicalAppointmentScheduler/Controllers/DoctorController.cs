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
        IConditionsManager conditionsManager;
        int pageSize = 5;
        int pageIndex = 1;

        public DoctorController()
        {
            SearchManager = new SearchManager();
            appointmentManager = new AppointmentManager();
            conditionsManager = new ConditionsManager();
        }

        public DoctorController(ISearchManager _SearchManager, IAppointmentManager _appointmentManager, IConditionsManager _conditionManager)
        {
            SearchManager = _SearchManager;
            appointmentManager = _appointmentManager;
            conditionsManager = _conditionManager;
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

            ViewBag.ShowDoctorDetails = false;
            ViewBag.ShowPatientDetails = true;

            return View(appointmentList.ToPagedList(pageIndex, pageSize));
        }

        public ActionResult GetAvailableSlots(DateTime date)
        {

            int doctorID = Convert.ToInt32(Session["LoggedInUser"]);
            var availableSlots = appointmentManager.GetAvailableSlots(doctorID, date);

            return Json(new { availableSlots }, JsonRequestBehavior.AllowGet);
        }


        public ActionResult EditAvailability()
        {
            return View();
        }
        
        public ActionResult MakeAppointment(DateTime date,int slotId,int allSlots)
        {
            bool isBooked=false;

            Appointment appointment = new Appointment();
            appointment.BookedBy = Convert.ToInt32(Session["LoggedInUser"]);
            appointment.DoctorID = Convert.ToInt32(Session["LoggedInUser"]);
            appointment.Details = "Not Available";
            appointment.Date = date;

            if (allSlots == 0) {
                appointment.SlotID = slotId;
                isBooked = appointmentManager.BookAppointment(appointment);
            }
            else{

                var availableSlots = appointmentManager.GetAvailableSlots(Convert.ToInt32(Session["LoggedInUser"]), date);
                foreach (var slot in availableSlots) {
                    appointment.SlotID = slot.ID;
                    isBooked = appointmentManager.BookAppointment(appointment);

                }

            }
            return Json(new { isBooked }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult ViewAppointmentHistory(int patientID, int? page)
        {
            pageIndex = page.HasValue ? Convert.ToInt32(page) : 1;
            List<Appointment> appointmentList = appointmentManager.GetPatientAppointmentHistory(patientID);
            return View(appointmentList.ToPagedList(pageIndex, pageSize));

        }

        public ActionResult ViewDetails(int patientID)
        {
            Patient patient = conditionsManager.GetDetails(patientID);
            List<Condition> listOfConditions = conditionsManager.GetConditions(patientID);
            return View(conditionsManager.GetDetails(patientID));
        }
    }
}