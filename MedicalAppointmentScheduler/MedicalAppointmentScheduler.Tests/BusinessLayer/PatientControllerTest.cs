using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Web.Mvc;
using MedicalAppointmentScheduler.Core.Data;
using MedicalAppointmentScheduler.Core.Business;
using MedicalAppointmentScheduler.Controllers;
using System.Data.Entity;
using System.Collections.Generic;
using System.Linq;
using Moq;
using EntityFramework.MoqHelper;

namespace MedicalAppointmentScheduler.Tests.BusinessLayer
{
    [TestClass]
    public class PatientControllerTest
    {
        private PatientController patientController;
        private Mock<MedicalSchedulerDBEntities> appointmentMock;
        private Mock<MedicalSchedulerDBEntities> slotsMock;
        private List<Appointment> appointmentData;
        private List<Slots> slotsData;

        [TestInitialize]
        public void setUpData()
        {
            appointmentData = new List<Appointment>();
            var mockAppointmentDetailsSet = EntityFrameworkMoqHelper.CreateMockForDbSet<Appointment>()
                                      .SetupForQueryOn(appointmentData)
                                       .WithAdd(appointmentData)
                                       .WithRemove(appointmentData)
                                       .WithFind(appointmentData, "ID");
            mockAppointmentDetailsSet.Setup(m => m.Include(It.IsAny<string>())).Returns(mockAppointmentDetailsSet.Object);

            appointmentMock = new Mock<MedicalSchedulerDBEntities>();
            appointmentMock.Setup(m => m.Appointments).Returns(mockAppointmentDetailsSet.Object);

            slotsData = new List<Slots>();
            var mockSlotsDetailsSet = EntityFrameworkMoqHelper.CreateMockForDbSet<Slots>()
                                    .SetupForQueryOn(slotsData)
                                    .WithAdd(slotsData)
                                    .WithRemove(slotsData)
                                    .WithFind(slotsData, "ID");
            mockSlotsDetailsSet.Setup(m => m.Include(It.IsAny<string>())).Returns(mockSlotsDetailsSet.Object);

            slotsMock = new Mock<MedicalSchedulerDBEntities>();
            slotsMock.Setup(m => m.Slots).Returns(mockSlotsDetailsSet.Object);

            patientController = new PatientController();
        }

        /**
         * As the index() function of the PatientController doesn't return anything, the same queries found
         * within the function are rewritten and tested below.
         */
        [TestMethod]
        public void TestIndex()
        {
            //Arrange
            Appointment user = new Appointment() { ID = 1, Details = "General Checkup", DoctorID = 3, PatientID = 2, BookedBy = 2, Date = new DateTime(2017, 03, 03), SlotID = 2 };
            appointmentData.Add(user);
            Appointment user2 = new Appointment() { ID = 2, Details = "General Checkup", DoctorID = 3, PatientID = 2, BookedBy = 4, Date = new DateTime(2017, 03, 23), SlotID = 3 };
            appointmentData.Add(user2);
            Appointment user3 = new Appointment() { ID = 3, Details = "Follow Up", DoctorID = 3, PatientID = 2, BookedBy = 4, Date = new DateTime(2017, 04, 15), SlotID = 3 };
            appointmentData.Add(user3);
            Appointment user4 = new Appointment() { ID = 4, Details = "General Checkup", DoctorID = 3, PatientID = 2, BookedBy = 2, Date = new DateTime(2017, 04, 28), SlotID = 2 };
            appointmentData.Add(user4);
            Appointment user5 = new Appointment() { ID = 5, Details = "Not Available", DoctorID = 3, PatientID = null, BookedBy = 3, Date = DateTime.Today, SlotID = 1 };
            appointmentData.Add(user5);

            Slots slot = new Slots() { ID = 1, StartTime = new TimeSpan(08,00,00), EndTime = new TimeSpan(08,45,00) };
            slotsData.Add(slot);
            Slots slot2 = new Slots() { ID = 2, StartTime = new TimeSpan(09, 00, 00), EndTime = new TimeSpan(09, 45, 00) };
            slotsData.Add(slot2);
            Slots slot3 = new Slots() { ID = 3, StartTime = new TimeSpan(10, 00, 00), EndTime = new TimeSpan(10, 45, 00) };
            slotsData.Add(slot3);

            //Act
            Dictionary<int, List<Slots>> times = new Dictionary<int, List<Slots>>();    //Holds the times for each appoinment
            List<Appointment> temp = appointmentData.Where(u => u.PatientID == 2).ToList(); //Retrieve all appointments for user

            //Gets the appointment times for each appointment
            foreach (Appointment i in temp)
            {
                times.Add(i.ID, slotsData.Where(u => u.ID == i.SlotID).ToList());
            }

            //Assert
            Assert.AreEqual(temp.Count, 4);
            Assert.AreEqual(1, temp[0].ID);
            Assert.AreEqual(2, temp[1].ID);
            Assert.AreEqual(3, temp[2].ID);
            Assert.AreEqual(4, temp[3].ID);
            Assert.AreEqual(true, temp[0].Date.Equals(new DateTime(2017, 03, 03)));
            Assert.AreEqual(true, temp[1].Date.Equals(new DateTime(2017, 03, 23)));
            Assert.AreEqual(true, temp[2].Date.Equals(new DateTime(2017, 04, 15)));
            Assert.AreEqual(true, temp[3].Date.Equals(new DateTime(2017, 04, 28)));
            Assert.AreEqual(2, times[temp[0].ID][0].ID);
            Assert.AreEqual(3, times[temp[1].ID][0].ID);
            Assert.AreEqual(3, times[temp[2].ID][0].ID);
            Assert.AreEqual(2, times[temp[3].ID][0].ID);
            Assert.AreEqual(true, times[temp[0].ID][0].StartTime.Equals(new TimeSpan(09, 00, 00)));
            Assert.AreEqual(true, times[temp[1].ID][0].StartTime.Equals(new TimeSpan(10, 00, 00)));
            Assert.AreEqual(true, times[temp[2].ID][0].StartTime.Equals(new TimeSpan(10, 00, 00)));
            Assert.AreEqual(true, times[temp[3].ID][0].StartTime.Equals(new TimeSpan(09, 00, 00)));


            //Act
            times = new Dictionary<int, List<Slots>>();    //Holds the times for each appoinment
            temp = appointmentData.Where(u => u.PatientID == 1).ToList(); //Retrieve all appointments for user

            //Gets the appointment times for each appointment
            foreach (Appointment i in temp)
            {
                times.Add(i.ID, slotsData.Where(u => u.ID == i.SlotID).ToList());
            }

            //Assert
            Assert.AreEqual(temp.Count, 0);
            Assert.AreEqual(times.Count, 0);
        }
    }
}
