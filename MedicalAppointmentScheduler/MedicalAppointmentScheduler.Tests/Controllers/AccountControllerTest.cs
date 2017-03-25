using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MedicalAppointmentScheduler;
using System.Web.Mvc;
using MedicalAppointmentScheduler.Controllers;
using MedicalAppointmentScheduler.Models;
/**
 * Unit test made for the purpose of testing AccountController
 * @author: Sean Fox
 * @date: 3/25/17
 */
namespace MedicalAppointmentScheduler.Tests.Controllers
{
    [TestClass]
    public class AccountControllerTest
    {

        /*
         * @param String returnURL
         */
        [TestMethod]
        public void Login1()
        {
            AccountController controller = new AccountController();
            ViewResult result = controller.Login("Create") as ViewResult;
            Assert.IsNotNull(result);
        }

        /*
         * @param LoginViewModel model
         * @param String returnURL
         */
        [TestMethod]
        public void Login2()
        {
            AccountController controller = new AccountController();
            string returnURL = "Create";
            LoginViewModel model = new LoginViewModel();
            model.Email = "sam123@uncc.edu";
            model.Password = "password";
            model.RememberMe = true;

            Assert.Equals(controller.Login(model,returnURL), Controller.View(model));
        }

        [TestMethod]
        public void VerifyCode(string provider, string returnUrl, bool rememberMe)
        {

            AccountController controller = new AccountController();

        }

        [TestMethod]
        public void VerifyCode(VerifyCodeViewModel model)
        {

            AccountController controller = new AccountController();

        }

        [TestMethod]
        public void Register()
        {

            AccountController controller = new AccountController();

        }

        [TestMethod]
        public void Register(RegisterViewModel model)
        {

            AccountController controller = new AccountController();

        }

        [TestMethod]
        public void ConfirmEmail(string userId, string code)
        {
            AccountController controller = new AccountController();
        }

        [TestMethod]
        public void ForgotPassword()
        {
            AccountController controller = new AccountController();
        }

        [TestMethod]
        public void ForgotPassword(ForgotPasswordViewModel model)
        {
            AccountController controller = new AccountController();
        }

        [TestMethod]
        public void ForgotPasswordConfirmation()
        {
            AccountController controller = new AccountController();
        }

        [TestMethod]
        public void ResetPassword(string code)
        {
            AccountController controller = new AccountController();
        }

        [TestMethod]
        public void ResetPasword(ResetPasswordViewModel model)
        {
            AccountController controller = new AccountController();
        }

        [TestMethod]
        public void ResetPaswordConfirmation()
        {
            AccountController controller = new AccountController();
        }

        [TestMethod]
        public void ExternalLogin(string provider, string returnUrl)
        {
            AccountController controller = new AccountController();
        }

        [TestMethod]
        public void SendCode(string returnUrl, bool rememberMe)
        {
            AccountController controller = new AccountController();
        }

        [TestMethod]
        public void SendCode(SendCodeViewModel model)
        {
            AccountController controller = new AccountController();
        }

        [TestMethod]
        public void ExternalLoginCallback(string returnUrl)
        {
            AccountController controller = new AccountController();
        }

        [TestMethod]
        public void ExternalLoginConfirmation(ExternalLoginConfirmationViewModel model, string returnUrl)
        {
            AccountController controller = new AccountController();
        }

        [TestMethod]
        public void LogOff()
        {
            AccountController controller = new AccountController();
        }

        [TestMethod]
        public void ExternalLoginFailure()
        {
            AccountController controller = new AccountController();
        }

        [TestMethod]
        public void Dispose(bool disposing)
        {
            AccountController controller = new AccountController();
        }
    }
}
