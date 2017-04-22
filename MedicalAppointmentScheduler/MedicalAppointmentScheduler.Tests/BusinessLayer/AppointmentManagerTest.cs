using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Web.Mvc;
using MedicalAppointmentScheduler.Core.Data;
using MedicalAppointmentScheduler.Core.Business;
using System.Data.Entity;
using System.Collections.Generic;
using System.Linq;
using Moq;
using EntityFramework.MoqHelper;

namespace MedicalAppointmentScheduler.Tests.BusinessLayer
{
    /// <summary>
    /// Summary description for AppoitmentManagerTest
    /// </summary>
    [TestClass]
    public class AppointmentManagerTest
    {
        private AppointmentManager appointmentManager;
        private Mock<MedicalSchedulerDBEntities> mockContext;
        List<Appointment> appointmentData;
        List<UserDetails> userDetailsData;
        List<Slots> slotsData;
      

        [TestInitialize]
        public void setUpData()
        {
            appointmentData = new List<Appointment>();
            var mockAppointmentSet = EntityFrameworkMoqHelper.CreateMockForDbSet<Appointment>()
                                     .SetupForQueryOn(appointmentData)
                                      .WithAdd(appointmentData)
                                      .WithRemove(appointmentData)
                                     .WithFind(appointmentData, "ID");

            userDetailsData = new List<UserDetails>();
            var mockUserDetailsSet = EntityFrameworkMoqHelper.CreateMockForDbSet<UserDetails>()
                                      .SetupForQueryOn(userDetailsData)
                                       .WithAdd(userDetailsData)
                                       .WithRemove(userDetailsData)
                                       .WithFind(userDetailsData, "ID");
            mockUserDetailsSet.Setup(m => m.Include(It.IsAny<string>())).Returns(mockUserDetailsSet.Object);

            slotsData = new List<Slots>();
            var mockSlotSet = EntityFrameworkMoqHelper.CreateMockForDbSet<Slots>()
                                     .SetupForQueryOn(slotsData)
                                      .WithAdd(slotsData)
                                      .WithRemove(slotsData)
                                     .WithFind(slotsData, "ID");

            mockContext = new Mock<MedicalSchedulerDBEntities>();
            mockContext.Setup(m => m.Appointments).Returns(mockAppointmentSet.Object);
            mockContext.Setup(m => m.UserDetails).Returns(mockUserDetailsSet.Object);
            mockContext.Setup(m => m.Slots).Returns(mockSlotSet.Object);       

            appointmentManager = new AppointmentManager(mockContext.Object);
        }


        [TestMethod]
        public void TestBookAppointment()
        {
            //Arrange
            Appointment testAppointment = new Appointment() { ID =1, BookedBy=4, Details="Test", Date=DateTime.Today, DoctorID=2, PatientID=9, SlotID=2 };

            //Act
            bool isBooked= appointmentManager.BookAppointment(testAppointment);

            //Assert
            Assert.AreEqual(true, isBooked);
            Assert.AreEqual(1, mockContext.Object.Appointments.Count());
            Assert.AreEqual("Test", appointmentData[0].Details);
            Assert.AreEqual(DateTime.Today, appointmentData[0].Date);
            Assert.AreEqual(2, appointmentData[0].SlotID);
            Assert.AreEqual(4, appointmentData[0].BookedBy);
        }

        [TestMethod]
        public void TestGetDoctorList()
        {
            //Arrange
            UserDetails testDoctor = new UserDetails() { ID = 3, FirstName = "Sean", LastName = "Fox", EmailAdress = "33@gmail.com", RoleID = 3 };
            userDetailsData.Add(testDoctor);

            //Act
            List<UserDetails>  testList = appointmentManager.GetDoctorList();

            Assert.AreEqual(1, testList.Count());         
            Assert.AreEqual("Sean", testList[0].FirstName);
            Assert.AreEqual("Fox", testList[0].LastName);
            Assert.AreEqual(3, testList[0].RoleID);
        }

        [TestMethod]
        public void TestGetDoctorListForNotDoctor()
        {
            //Arrange
            UserDetails testDoctor = new UserDetails() { ID = 3, FirstName = "Sean", LastName = "Fox", EmailAdress = "33@gmail.com", RoleID = 2 };
            userDetailsData.Add(testDoctor);

            //Act
            List<UserDetails> testList = appointmentManager.GetDoctorList();

            Assert.AreEqual(0, testList.Count());
           
        }

        [TestMethod]
        public void GetAvailableSlots()
        {
            //Arrange
            Slots testslot1 = new Slots() { ID = 1, EndTime = DateTime.Now.TimeOfDay, StartTime = DateTime.Now.TimeOfDay };
            slotsData.Add(testslot1);
            Slots testslot2 = new Slots() { ID = 2, EndTime = DateTime.Now.TimeOfDay, StartTime = DateTime.Now.TimeOfDay };
            slotsData.Add(testslot2);

            //Booked slot
            Appointment testAppointment1 = new Appointment() { ID = 1, BookedBy = 4, Details = "Test1", Date = new DateTime(2018,12,01), DoctorID = 2, PatientID = 9, SlotID = 2};
            appointmentData.Add(testAppointment1);

            //Cancelled slot
            Appointment testAppointment2 = new Appointment() { ID = 2, BookedBy = 4, Details = "Test2", Date = new DateTime(2018, 12, 01), DoctorID = 2, PatientID = 9, SlotID = 1, IsCancelled=true };
            appointmentData.Add(testAppointment2);

            //Act
            List<AvailableSlots> slots = appointmentManager.GetAvailableSlots(2, new DateTime(2018, 12, 01));

            //Assert
            Assert.AreEqual(1, slots.Count());
            Assert.AreEqual(1, slots[0].ID);
        }

        [TestMethod]
        public void TestFindAppointment()
        {
            //Arrange
            Appointment testAppointment = new Appointment() { ID = 1, BookedBy = 4, Details = "Test", Date = new DateTime(2018, 12, 01), DoctorID = 2, PatientID = 9, SlotID = 2 };
            appointmentData.Add(testAppointment);

            //Act
            Appointment appointment = appointmentManager.FindAppointment(1);

            //Assert 
            Assert.AreEqual(1, appointment.ID);
            Assert.AreEqual("Test", appointment.Details);
            Assert.AreEqual(4, appointment.BookedBy);
        }

        [TestMethod]
        public void TestDeleteAppointment()
        {
            Appointment testAppointment = new Appointment() { ID = 1, BookedBy = 4, Details = "Test", Date = new DateTime(2018, 12, 01), DoctorID = 2, PatientID = 9, SlotID = 2 };
            appointmentData.Add(testAppointment);

            //PreAssert
            Assert.AreEqual(false, appointmentData[0].IsCancelled);
            //Act
            appointmentManager.DeleteAppointment(1);

            Assert.AreEqual(true, appointmentData[0].IsCancelled);
        }

        [TestMethod]
        public void TestGetAppointmentList()
        {
            //Arrange
                Appointment testAppointment1 = new Appointment() { ID = 1, BookedBy = 4, Details = "Test 1", Date = new DateTime(2018, 12, 01), DoctorID = 2, PatientID = 9, L_Slots= new Slots {ID=1, StartTime= new TimeSpan(11,0,0) } };
                appointmentData.Add(testAppointment1);

                Appointment testAppointment2 = new Appointment() { ID = 2, BookedBy = 4, Details = "Test 2", Date = new DateTime(2018, 12, 01), DoctorID = 2, PatientID = 9, L_Slots = new Slots { ID = 2, StartTime = new TimeSpan(9, 0, 0) } };
                appointmentData.Add(testAppointment2);

                //past appointment
                Appointment testAppointment3 = new Appointment() { ID = 3, BookedBy = 4, Details = "Test 3", Date = new DateTime(2016, 12, 01), DoctorID = 2, PatientID = 9, L_Slots = new Slots { ID = 5, StartTime = new TimeSpan(18, 0, 0) } };
                appointmentData.Add(testAppointment3);
          
                //Cancelled appointment
                Appointment testAppointment4 = new Appointment() { ID = 4, BookedBy = 4, Details = "Test 4", Date = new DateTime(2018, 12, 01), DoctorID = 2, PatientID = 9, IsCancelled=true, L_Slots = new Slots { ID = 3, StartTime = new TimeSpan(10, 0, 0) } };
                appointmentData.Add(testAppointment4);

            //Act
                List<Appointment> appointments = appointmentManager.GetAppointmentList();

            //Assert
                Assert.AreEqual(2, appointments.Count);
                //Test2 should come first due to early start time
                Assert.AreEqual("Test 2", appointments[0].Details);
                Assert.AreEqual("Test 1", appointments[1].Details);
        }

        [TestMethod]
        public void TestGetPatientAppointmentHistory()
        {
            //Arrange
                Appointment testAppointment1 = new Appointment() { ID = 1, BookedBy = 4, Details = "Test 1", Date = new DateTime(2016, 12, 01), DoctorID = 2, PatientID = 9, L_Slots = new Slots { ID = 1, StartTime = new TimeSpan(11, 0, 0) } };
                appointmentData.Add(testAppointment1);

                Appointment testAppointment2 = new Appointment() { ID = 2, BookedBy = 4, Details = "Test 2", Date = new DateTime(2016, 12, 01), DoctorID = 2, PatientID = 9, L_Slots = new Slots { ID = 2, StartTime = new TimeSpan(9, 0, 0) } };
                appointmentData.Add(testAppointment2);

                //past appointment of different patient
                Appointment testAppointment3 = new Appointment() { ID = 3, BookedBy = 4, Details = "Test 3", Date = new DateTime(2016, 12, 01), DoctorID = 2, PatientID = 2, L_Slots = new Slots { ID = 5, StartTime = new TimeSpan(18, 0, 0) } };
                appointmentData.Add(testAppointment3);

                //Cancelled appointment
                Appointment testAppointment4 = new Appointment() { ID = 4, BookedBy = 4, Details = "Test 4", Date = new DateTime(2016, 12, 02), DoctorID = 2, PatientID = 9, IsCancelled = true, L_Slots = new Slots { ID = 3, StartTime = new TimeSpan(8, 0, 0) } };
                appointmentData.Add(testAppointment4);

            //Act
                List<Appointment> appointments = appointmentManager.GetPatientAppointmentHistory(9);

            //Assert
                Assert.AreEqual(3, appointments.Count);

                //Test2 should come first due to early start time
                Assert.AreEqual("Test 2", appointments[0].Details);
                Assert.AreEqual("Test 1", appointments[1].Details);
                Assert.AreEqual("Test 4", appointments[2].Details);
        }

        [TestMethod]
        public void TestGetUpComingAppointment()
        {
            //Arrange
                Appointment testAppointment1 = new Appointment() { ID = 1, BookedBy = 4, Details = "Test 1", Date = new DateTime(2018, 12, 01), DoctorID = 2, PatientID = 9, L_Slots = new Slots { ID = 1, StartTime = new TimeSpan(11, 0, 0) } };
                appointmentData.Add(testAppointment1);

                //past appointment
                Appointment testAppointment2 = new Appointment() { ID = 2, BookedBy = 4, Details = "Test 2", Date = new DateTime(2016, 12, 01), DoctorID = 2, PatientID = 9, L_Slots = new Slots { ID = 2, StartTime = new TimeSpan(9, 0, 0) } };
                appointmentData.Add(testAppointment2);

                // appointment of different patient
                Appointment testAppointment3 = new Appointment() { ID = 3, BookedBy = 4, Details = "Test 3", Date = new DateTime(2016, 12, 01), DoctorID = 2, PatientID = 2, L_Slots = new Slots { ID = 5, StartTime = new TimeSpan(18, 0, 0) } };
                appointmentData.Add(testAppointment3);

                //Cancelled appointment
                Appointment testAppointment4 = new Appointment() { ID = 4, BookedBy = 4, Details = "Test 4", Date = new DateTime(2018, 12, 01), DoctorID = 2, PatientID = 9, IsCancelled = true, L_Slots = new Slots { ID = 3, StartTime = new TimeSpan(10, 0, 0) } };
                appointmentData.Add(testAppointment4);

            //Act
                List<Appointment> appointments = appointmentManager.GetUpcomingAppointments(9);

            //Assert
                Assert.AreEqual(1, appointments.Count);
                //Test2 should come first due to early start time
                Assert.AreEqual("Test 1", appointments[0].Details);
            
        }
    }


}
