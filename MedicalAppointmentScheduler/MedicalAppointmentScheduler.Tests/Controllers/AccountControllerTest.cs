using MedicalAppointmentScheduler.Controllers;
using System;
using System.Text;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MedicalAppointmentScheduler.Core.Data;
using System.Linq;
using Moq;
using System.Data.Entity;
using System.Web.Mvc;
using System.Web.Security;
using System.Web;

namespace MedicalAppointmentScheduler.Tests.Controllers
{
    /// <summary>
    /// Summary description for AccountControllerTest
    /// </summary>
    [TestClass]
    public class AccountControllerTest
    {
        AccountController accountController;

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

            accountController = new AccountController(mockContext.Object);
        }

        [TestMethod]
        public void TestLoginGet()
        {
            string testurl = "http://gooogle.com";
            var result = accountController.Login(testurl) as ViewResult;

            Assert.AreEqual(testurl, result.ViewBag.ReturnUrl);
        }

        [TestMethod]
        public void TestLoginValidUser()
        {
            //Response.Cookies.Add(FormsAuthentication.GetAuthCookie("user-1", true));
            UserLogin loginViewModel = new UserLogin() { Email = "aa@gmail.com", Password = "12345" };
            //var result = accountController.Login(loginViewModel) as ViewResult;

            //Assert.AreEqual("Index", result.ViewName);
            //Assert.AreEqual("Administrator", result.Model);
        }
    }
}
