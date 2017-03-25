using System.Web.Mvc;
using System.Web.Security;
using MedicalAppointmentScheduler.Models;
using MedicalAppointmentScheduler.Models.BusinessClass;
using MedicalAppointmentScheduler.Core.Business;
using MedicalAppointmentScheduler.Core.Data;

namespace MedicalAppointmentScheduler.Controllers
{
    public class AccountController : Controller
    {
        //GET: this action is called for all anonymous users to get authenticated
       [AllowAnonymous]
        public ActionResult Login(string returnUrl)
        {
            ViewBag.ReturnUrl = returnUrl;
            return View();
        }

        //POST: Authenticate the user and redirect to action based on its role
        [HttpPost]
        public ActionResult Login(UserLogin loginViewModel)
        {
            if (ModelState.IsValid)
            {
                AccountManager loginManager = new AccountManager();
                int userId = loginManager.ValidatedUser(loginViewModel.Email, loginViewModel.Password);
                if (userId != 0)
                {
                    FormsAuthentication.SetAuthCookie(loginViewModel.Email, false);
                    return RedirectToAction("Index", loginManager.GetUserRole(userId));
                }
                else
                {
                    ModelState.AddModelError("", "The user login or password provided is incorrect.");
                }

            }
            // If we got this far, something failed, redisplay form
            return View(loginViewModel);
        }

        // POST: /Account/LogOff
        [HttpPost]
        [Authorize]
        public ActionResult LogOff()
        {
            FormsAuthentication.SignOut();
            return RedirectToAction("Index", "Home");
        }

    }
}
