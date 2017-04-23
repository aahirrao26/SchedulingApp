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
using EntityFramework.MoqHelper;
using System.Web;

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
            var userLoginData = new List<UserLogin>
            {
                new UserLogin { ID = 1, Email="aa@gmail.com", Password="12345", UserID = 1},
                new UserLogin { ID = 2, Email="bb@gmail.com", Password="12345", UserID = 2}
            };

            var userRoleData = new List<UserRole>
            {
                new UserRole {ID=1, RoleName="Test_Admin"}
            };

            var userDetailsData = new List<UserDetails>
            {
                new UserDetails {ID=1,RoleID=1, EmailAdress="aa@gmail.com" },
                new UserDetails {ID=2,RoleID=2, EmailAdress="bb@gmail.com"}
            };

            var mockUserLoginSet = EntityFrameworkMoqHelper.CreateMockForDbSet<UserLogin>()
                                      .SetupForQueryOn(userLoginData);

            var mockUserRoleSet = EntityFrameworkMoqHelper.CreateMockForDbSet<UserRole>()
                                      .SetupForQueryOn(userRoleData);              

            var mockUserDetailsSet = EntityFrameworkMoqHelper.CreateMockForDbSet<UserDetails>()
                                      .SetupForQueryOn(userDetailsData);
          

            var mockContext = new Mock<MedicalSchedulerDBEntities>();
            mockContext.Setup(m => m.UserLogins).Returns(mockUserLoginSet.Object);
            mockContext.Setup(m => m.UserDetails).Returns(mockUserDetailsSet.Object);
            mockContext.Setup(m => m.UserRoles).Returns(mockUserRoleSet.Object);

            accountManager = new AccountManager(mockContext.Object);
            mockAccountManager = new Mock<IAccountManager>();
            controller = new AccountController(mockAccountManager.Object, mockAuth.Object);            
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
            Assert.AreEqual("Test_Admin", role);
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

            //Arrange session object
            var context = new Mock<ControllerContext>();
            var session = new Mock<HttpSessionStateBase>();
            context.Setup(m => m.HttpContext.Session).Returns(session.Object);
            controller.ControllerContext = context.Object;

            //Act
            RedirectToRouteResult result = (RedirectToRouteResult)controller.Login(loginViewModel) ;

            //Assert
            Assert.AreEqual("Index", result.RouteValues["action"]);
            Assert.AreEqual("Administrator", result.RouteValues["controller"]);
        }

        [TestMethod]
        public void TestIsUserInRole_Authorize()
        {
            bool authorized = accountManager.IsUserInRole("aa@gmail.com",1);
            Assert.AreEqual(true, authorized);
        }

        [TestMethod]
        public void TestIsUserInRole_Unauthorize()
        {
            bool authorized = accountManager.IsUserInRole("bb@gmail.com", 1);
            Assert.AreEqual(false, authorized);
        }
    }
}
