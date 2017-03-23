using System;
using System.Globalization;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;
using MedicalAppointmentScheduler.Models;
using System.Web.Security;

namespace MedicalAppointmentScheduler.Controllers
{

    public class AccountController : Controller
    {
        // GET: Login
        [AllowAnonymous]
        public ActionResult Login(string returnUrl)
        {
            ViewBag.ReturnUrl = returnUrl;
            return View();
        }

        [HttpPost]
        public ActionResult Login(LoginViewModel loginViewModel)
        {
            //if (ModelState.IsValid)
            //{
            //    AccountManager loginManager = new AccountManager();

            //    if (loginManager.ValidatedUser(loginViewModel.Email, loginViewModel.Password))
            //    {
            //        FormsAuthentication.SetAuthCookie(loginViewModel.Email, false);
            //        return View("Index");
            //    }
            //    else
            //    {
            //        ModelState.AddModelError("", "The user login or password provided is incorrect.");
            //    }

            //}
            //// If we got this far, something failed, redisplay form
            return View(loginViewModel);
        }
    }
}
