using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Web.Mvc;
using MedicalAppointmentScheduler.Core.Data;
using MedicalAppointmentScheduler.Core.Business;
using MedicalAppointmentScheduler.Models.BusinessClass;
using System.Data.Entity;
using System.Collections.Generic;
using System.Linq;
using Moq;

namespace MedicalAppointmentScheduler.Tests.Controllers
{
    [TestClass]
    public class AdminManagerTest
    {
        private AdminManager adminManager;

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

            adminManager = new AdminManager(mockContext.Object);
        }

        [TestMethod]
        public void CreateUserTest() {
            UserDetails user = new UserDetails();
            user.ID = 2;
            user.FirstName = "Sean";
            user.LastName = "Fox";
            user.EmailAdress = "33@gmail.com"; 
            user.RoleID = 3;
            adminManager.CreateUser(user);

            var test = adminManager.getUserDetails(user.ID);
            Assert.AreEqual(2, test);
        }

        [TestMethod]
        public void Delete()
        {

        }
    }
}