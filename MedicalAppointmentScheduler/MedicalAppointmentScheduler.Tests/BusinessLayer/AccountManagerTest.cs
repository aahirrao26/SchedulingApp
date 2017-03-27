using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System.Data.Entity;
using MedicalAppointmentScheduler.Core.Data;
using System.Collections.Generic;
using System.Linq;
using MedicalAppointmentScheduler.Controllers;
using MedicalAppointmentScheduler.Core.Business;
using System.Web.Mvc;

namespace MedicalAppointmentScheduler.Tests.BusinessLayer
{
    [TestClass]
    public class AccountManagerTest
    {
        AccountManager accountManager;
        AccountController controller;

        Mock<IAccountManager> mockAccountManager;      
        Mock<IAuthentication> mockAuth = new Mock<IAuthentication>();

        [TestInitialize]
        public void setUpData()
        {
            var UserLoginData = new List<UserLogin>
            {
                new UserLogin { ID = 1, Email="aa@gmail.com", Password="12345", UserID = 1},
                new UserLogin { ID = 2, Email="bb@gmail.com", Password="12345", UserID = 2}
            }.AsQueryable();

            var userRoleData = new List<UserRole>
            {
                new UserRole {ID=1, RoleName="Administrator"}
            }.AsQueryable();

            var userDetailsData = new List<UserDetails>
            {
                new UserDetails {ID=1,RoleID=1}
            }.AsQueryable();

            var mockUserLoginSet = new Mock<DbSet<UserLogin>>();
            mockUserLoginSet.As<IQueryable<UserLogin>>().Setup(m => m.Provider).Returns(UserLoginData.Provider);
            mockUserLoginSet.As<IQueryable<UserLogin>>().Setup(m => m.Expression).Returns(UserLoginData.Expression);
            mockUserLoginSet.As<IQueryable<UserLogin>>().Setup(m => m.ElementType).Returns(UserLoginData.ElementType);

            var mockUserRoleSet = new Mock<DbSet<UserRole>>();
            mockUserRoleSet.As<IQueryable<UserRole>>().Setup(m => m.Provider).Returns(userRoleData.Provider);
            mockUserRoleSet.As<IQueryable<UserRole>>().Setup(m => m.Expression).Returns(userRoleData.Expression);
            mockUserRoleSet.As<IQueryable<UserRole>>().Setup(m => m.ElementType).Returns(userRoleData.ElementType);

            var mockUserDetailsSet = new Mock<DbSet<UserDetails>>();
            mockUserDetailsSet.As<IQueryable<UserDetails>>().Setup(m => m.Provider).Returns(userDetailsData.Provider);
            mockUserDetailsSet.As<IQueryable<UserDetails>>().Setup(m => m.Expression).Returns(userDetailsData.Expression);
            mockUserDetailsSet.As<IQueryable<UserDetails>>().Setup(m => m.ElementType).Returns(userDetailsData.ElementType);

            var mockContext = new Mock<MedicalSchedulerDBEntities>();
            mockContext.Setup(m => m.UserLogins).Returns(mockUserLoginSet.Object);
            mockContext.Setup(m => m.UserDetails).Returns(mockUserDetailsSet.Object);
            mockContext.Setup(m => m.UserRoles).Returns(mockUserRoleSet.Object);

            accountManager = new AccountManager(mockContext.Object);
            mockAccountManager = new Mock<IAccountManager>();
            controller = new AccountController(mockContext.Object, mockAccountManager.Object, mockAuth.Object);            
        }

        [TestMethod]
        public void TestValidateUserForValidCredentials()
        {           
            int userId =  accountManager.ValidateUser("aa@gmail.com","12345");

            Assert.AreEqual(1, userId);
        }

        [TestMethod]
        public void TestValidateUserForInvalidEmail()
        {
            int userId = accountManager.ValidateUser("ba@gmail.com", "12345");

            Assert.AreEqual(0, userId);
        }

        [TestMethod]
        public void TestValidateUserForInvalidPassword()
        {
            int userId = accountManager.ValidateUser("aa@gmail.com", "xyz");

            Assert.AreEqual(0, userId);
        }

        [TestMethod]
        public void TestGetUserRole()
        {
            string role = accountManager.GetUserRole(1);
            Assert.AreEqual("Administrator", role);
        }

        [TestMethod]
        public void TestGetUserInvalidRole()
        {
            string role = accountManager.GetUserRole(2);
            Assert.AreEqual(null, role);
        }

        [TestMethod]
        public void TestControllerLogin()
        {   
            //Arrange
            UserLogin loginViewModel = new UserLogin() { Email="aa@gmail.com",Password="12345"};
            mockAuth.Setup(x => x.SetAuthCookie(loginViewModel.Email)).Verifiable();

            mockAccountManager.Setup(x => x.ValidateUser(loginViewModel.Email, loginViewModel.Password)).Returns(1);
            mockAccountManager.Setup(x => x.GetUserRole(1)).Returns("Administrator");

            //Act
            RedirectToRouteResult result = (RedirectToRouteResult)controller.Login(loginViewModel) ;

            //Assert
            Assert.AreEqual("Index", result.RouteValues["action"]);
            Assert.AreEqual("Administrator", result.RouteValues["controller"]);
        }
    }
}
