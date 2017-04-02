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
    public class AdminManagerTest
    {
        private AdminManager adminManager;
        private Mock<MedicalSchedulerDBEntities> mockContext;
        List<UserDetails> userDetailsData;
        List<UserLogin> userLoginData;
        List<UserAddress> userAddressData;
        List<UserRole> userRoleData;

       [TestInitialize]
        public void setUpData()
        {
             userLoginData = new List<UserLogin>();
             var mockUserLoginSet = EntityFrameworkMoqHelper.CreateMockForDbSet<UserLogin>()
                                      .SetupForQueryOn(userLoginData)
                                       .WithAdd(userLoginData)
                                       .WithRemove(userLoginData)
                                       .WithFind(userLoginData,"UserID");
        
            userDetailsData = new List<UserDetails>();
            var mockUserDetailsSet = EntityFrameworkMoqHelper.CreateMockForDbSet<UserDetails>()
                                      .SetupForQueryOn(userDetailsData)
                                       .WithAdd(userDetailsData)
                                       .WithRemove(userDetailsData)
                                       .WithFind(userDetailsData,"ID");
            mockUserDetailsSet.Setup(m => m.Include(It.IsAny<string>())).Returns(mockUserDetailsSet.Object);
                                          
            userAddressData = new List<UserAddress>();
            var mockUserAddressSet = EntityFrameworkMoqHelper.CreateMockForDbSet<UserAddress>()
                                      .SetupForQueryOn(userAddressData)
                                       .WithAdd(userAddressData)
                                       .WithRemove(userAddressData)
                                       .WithFind(userAddressData,"UserID");

            userRoleData = new List<UserRole>();
            userRoleData.Add(new UserRole { ID = 1, RoleName = "Test_Admin" });
            userRoleData.Add(new UserRole { ID = 2, RoleName = "Test_Doctor" });

            var mockUserRoleSet = EntityFrameworkMoqHelper.CreateMockForDbSet<UserRole>()
                                      .SetupForQueryOn(userRoleData);


           mockContext = new Mock<MedicalSchedulerDBEntities>();        
            mockContext.Setup(m => m.UserLogins).Returns(mockUserLoginSet.Object);
            mockContext.Setup(m => m.UserDetails).Returns(mockUserDetailsSet.Object);
            mockContext.Setup(m => m.UserAddresses).Returns(mockUserAddressSet.Object);
            mockContext.Setup(m => m.UserRoles).Returns(mockUserRoleSet.Object);

            adminManager = new AdminManager(mockContext.Object);    
        }

        [TestMethod]
        public void CreateUserTest()
        {
            //Arrange
            UserDetails user = new UserDetails() { ID = 3, FirstName = "Sean", LastName = "Fox", EmailAdress = "33@gmail.com", RoleID = 3 };

            //Act
            adminManager.CreateUser(user);

            //Assert
            Assert.AreEqual(1, mockContext.Object.UserDetails.Count());
            Assert.AreEqual("33@gmail.com", mockContext.Object.UserDetails.Single().EmailAdress);
            Assert.AreEqual("Sean", userDetailsData[0].FirstName);
            Assert.AreEqual("Fox", userDetailsData[0].LastName);
            Assert.AreEqual(3, userDetailsData[0].RoleID);
        }

        [TestMethod]
        public void DeleteUserTest()
        {
            //Arrange
            UserDetails testUser = new UserDetails() { ID = 3, FirstName = "Sean", LastName = "Fox", EmailAdress = "33@gmail.com", RoleID = 3 };
            userDetailsData.Add(testUser);
            UserAddress testAddress = new UserAddress() { AddressID = 1, AddressLine = "5th Street", City = "Charlotte", StateID = 1, UserID = 3 };
            userAddressData.Add(testAddress);
            UserLogin userLogin = new UserLogin { ID = 1, UserID=3, Email="33@gmail.com", Password="xyz"};
            userLoginData.Add(userLogin);
            
            //Pre-Assert          
            Assert.AreEqual(1, userDetailsData.Count);
            Assert.AreEqual(1, userAddressData.Count);
            Assert.AreEqual(1, userLoginData.Count);


            //Act
            adminManager.DeleteUser(3);
           
            //Assert
            Assert.AreEqual(0,userDetailsData.Count);
            Assert.AreEqual(0, userAddressData.Count);
            Assert.AreEqual(0, userLoginData.Count);
        }

        [TestMethod]

        public void FindUserTest()
        {
            //Arrange
            Assert.AreEqual(0, mockContext.Object.UserDetails.Count());
            UserDetails user = new UserDetails() { ID = 1, FirstName = "Sean", LastName = "Fox", EmailAdress = "33@gmail.com", RoleID = 3 };
            userDetailsData.Add(user);
            Assert.AreEqual(1, mockContext.Object.UserDetails.Count());

            //Act
            UserDetails testUser = adminManager.FindUser(1);

            //Assert
            Assert.AreEqual("33@gmail.com", testUser.EmailAdress);
            Assert.AreEqual("Sean", testUser.FirstName);
            Assert.AreEqual("Fox", testUser.LastName);
        }

        [TestMethod]
        public void TestGetRoles()
        {
            List<UserRole> testRoles = adminManager.GetRoles().ToList();

            Assert.AreEqual("Test_Admin",testRoles[0].RoleName);
            Assert.AreEqual("Test_Doctor",testRoles[1].RoleName);
        }

        [TestMethod]
        public void TestGetUserList()
        {
            //Arrange            
            UserDetails user = new UserDetails() { ID = 1, FirstName = "John", LastName = "Test", EmailAdress = "33@gmail.com", RoleID = 3, L_User_Roles= new UserRole { ID = 3, RoleName = "Test_Admin" } };
            userDetailsData.Add(user);
                       
            //Act
            List<UserDetails> testUsers = adminManager.GetUserList();

            //Assert
            Assert.AreEqual("John", testUsers[0].FirstName);
            Assert.AreEqual("Test", testUsers[0].LastName);
        }

        /// <summary>
        /// The below method is testing the edit user functionality with test database
        /// </summary>
        [TestMethod]
        public void TestEditUser()
        {
            //Arrange
            //The below will initialize accordingly to the connection string specified in the app.config under test project
            //the connection string under test project is set to TestDatabase
            MedicalSchedulerDBEntities testContext = new MedicalSchedulerDBEntities();
            AdminManager testAdminManager = new AdminManager(testContext);

            //Edit test for first name, Last name, Email and phone.
            UserDetails editUser = new UserDetails() { ID = 4, FirstName = "Test4 Edit", LastName = "Test4 Edit", EmailAdress = "edit_test4@gmail.com", RoleID = 4, L_User_Roles = null, User_Address = null, User_Login = null, Phone = "454-133-1234" };

            testAdminManager.EditUser(editUser);

            UserDetails testUser = testAdminManager.FindUser(4);
            Assert.AreEqual("Test4 Edit", testUser.FirstName);
            Assert.AreEqual("Test4 Edit", testUser.LastName);
        }
    }
}