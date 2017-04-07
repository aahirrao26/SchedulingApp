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
    [TestClass]
    public class SearchManagerTest
    {
        private SearchManager SearchManager;
        private Mock<MedicalSchedulerDBEntities> mockContext;
        private List<UserDetails> userDetailsData;

        [TestInitialize]
        public void setUpData()
        {
            userDetailsData = new List<UserDetails>();
            var mockUserDetailsSet = EntityFrameworkMoqHelper.CreateMockForDbSet<UserDetails>()
                                      .SetupForQueryOn(userDetailsData)
                                       .WithAdd(userDetailsData)
                                       .WithRemove(userDetailsData)
                                       .WithFind(userDetailsData, "ID");
            mockUserDetailsSet.Setup(m => m.Include(It.IsAny<string>())).Returns(mockUserDetailsSet.Object);

            mockContext = new Mock<MedicalSchedulerDBEntities>();
            mockContext.Setup(m => m.UserDetails).Returns(mockUserDetailsSet.Object);

            SearchManager = new SearchManager(mockContext.Object);
        }

        [TestMethod]
        public void TestGetPatientList()
        {
            //Arrange
            UserDetails user = new UserDetails() { ID = 1, FirstName = "James", LastName = "Bond", Phone = "980-234-6565",EmailAdress = "jbond@gmail.com", RoleID = 1 };
            userDetailsData.Add(user);
            UserDetails user2 = new UserDetails() { ID = 2, FirstName = "John", LastName = "Smith", Phone = "704-495-1839", EmailAdress = "johnsmith@gmail.com", RoleID = 2 };
            userDetailsData.Add(user2);
            UserDetails user3 = new UserDetails() { ID = 3, FirstName = "John", LastName = "Cena", Phone = "678-123-4567", EmailAdress = "jcena@ymail.com", RoleID = 3 };
            userDetailsData.Add(user3);
            UserDetails user4 = new UserDetails() { ID = 4, FirstName = "Bernie", LastName = "Sanders", Phone = "454-133-5642", EmailAdress = "feelthebern@yahoo.com", RoleID = 4 };
            userDetailsData.Add(user4);

            //Act
            List<UserDetails> testPatients = SearchManager.GetPatientList("John", "Smith");

            //Assert
            Assert.AreEqual("John", testPatients[0].FirstName);
            Assert.AreEqual("Smith", testPatients[0].LastName);
            Assert.AreEqual(1, testPatients.Count);

            //Act
            testPatients = SearchManager.GetPatientList("John", "");

            //Assert
            Assert.AreEqual("John", testPatients[0].FirstName);
            Assert.AreEqual("Smith", testPatients[0].LastName);
            Assert.AreEqual(1, testPatients.Count);
        }
    }
}
