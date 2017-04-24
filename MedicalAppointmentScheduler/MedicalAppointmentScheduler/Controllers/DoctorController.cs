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
            ViewBag.PateintID = patientID;
            return View(appointmentList.ToPagedList(pageIndex, pageSize));
        }

        public ActionResult ViewDetails(int patientID)
        {
            String typeOne = conditionsManager.GetTypes(1);
            ViewBag.typeOne = typeOne.ToString();

            String typeTwo = conditionsManager.GetTypes(2);
            ViewBag.typeTwo = typeTwo;

            String typeThree = conditionsManager.GetTypes(3);
            ViewBag.typeThree = typeThree;

            String typeFour = conditionsManager.GetTypes(4);
            ViewBag.typeFour = typeFour;

            String typeFive = conditionsManager.GetTypes(5);
            ViewBag.typeFive = typeFive;

            String typeSix = conditionsManager.GetTypes(6);
            ViewBag.typeSix = typeSix;

            String typeSeven = conditionsManager.GetTypes(7);
            ViewBag.typeSeven = typeSeven;

            String typeEight = conditionsManager.GetTypes(8);
            ViewBag.typeEight = typeEight;

            String typeNine = conditionsManager.GetTypes(9);
            ViewBag.typeNine = typeNine;

            String typeTen = conditionsManager.GetTypes(10);
            ViewBag.typeTen = typeTen;

            String typeEleven = conditionsManager.GetTypes(11);
            ViewBag.typeEleven = typeEleven;

            String typeTwelve = conditionsManager.GetTypes(12);
            ViewBag.typeTwelve = typeTwelve;

            String typeThirteen = conditionsManager.GetTypes(13);
            ViewBag.typeThirteen = typeThirteen;

            String typeFourten = conditionsManager.GetTypes(14);
            ViewBag.typeFourten = typeFourten;

            String typeFifthteen = conditionsManager.GetTypes(15);
            ViewBag.typeFifthteen = typeFifthteen;

            String typeSixteen = conditionsManager.GetTypes(16);
            ViewBag.typeSixteen = typeSixteen;


            Patient patient = conditionsManager.GetDetails(patientID);
            ViewBag.patient = patient;


            List<String> listOfTypeOne = conditionsManager.GetConditions(patientID, 1);
            ViewBag.listOfTypeOne = listOfTypeOne;

            List<String> listOfTypeTwo = conditionsManager.GetConditions(patientID, 2);
            ViewBag.listOfTypeTwo = listOfTypeTwo;

            List<String> listOfTypeThree = conditionsManager.GetConditions(patientID, 3);
            ViewBag.listOfTypeThree = listOfTypeThree;

            List<String> listOfTypeFour = conditionsManager.GetConditions(patientID, 4);
            ViewBag.listOfTypeFour = listOfTypeFour;

            List<String> listOfTypeFive = conditionsManager.GetConditions(patientID, 5);
            ViewBag.listOfTypeFive = listOfTypeFive;

            List<String> listOfTypeSix = conditionsManager.GetConditions(patientID, 6);
            ViewBag.listOfTypeSix = listOfTypeSix;

            List<String> listOfTypeSeven = conditionsManager.GetConditions(patientID, 7);
            ViewBag.listOfTypeSeven = listOfTypeSeven;

            List<String> listOfTypeEight = conditionsManager.GetConditions(patientID, 8);
            ViewBag.listOfTypeEight = listOfTypeEight;

            List<String> listOfTypeNine = conditionsManager.GetConditions(patientID, 9);
            ViewBag.listOfTypeNine = listOfTypeNine;

            List<String> listOfTypeTen = conditionsManager.GetConditions(patientID, 10);
            ViewBag.listOfTypeTen = listOfTypeTen;

            List<String> listOfTypeEleven = conditionsManager.GetConditions(patientID, 11);
            ViewBag.listOfTypeEleven = listOfTypeEleven;

            List<String> listOfTypeTwelve = conditionsManager.GetConditions(patientID, 12);
            ViewBag.listOfTypeTwelve = listOfTypeTwelve;

            List<String> listOfTypeThirteen = conditionsManager.GetConditions(patientID, 13);
            ViewBag.listOfTypeThirteen = listOfTypeThirteen;

            List<String> listOfTypeFourteen = conditionsManager.GetConditions(patientID, 14);
            ViewBag.listOfTypeFourteen = listOfTypeFourteen;

            List<String> listOfTypeFifthteen = conditionsManager.GetConditions(patientID, 15);
            ViewBag.listOfTypeFifthteen = listOfTypeFifthteen;

            List<String> listOfTypeSixteen = conditionsManager.GetConditions(patientID, 16);
            ViewBag.listOfTypeSixteen = listOfTypeSixteen;

            return View();
        }
    }
}