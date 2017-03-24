using System.Web.Mvc;
using System.Web.Security;
using MedicalAppointmentScheduler.Models;
using MedicalAppointmentScheduler.Models.BusinessClass;

namespace MedicalAppointmentScheduler.Controllers
{

    public class AccountController : Controller
    {
        //GET: Login
       [AllowAnonymous]
        public ActionResult Login(string returnUrl)
        {
            ViewBag.ReturnUrl = returnUrl;
            return View();
        }

        //Authenticate the user 
        [HttpPost]
        public ActionResult Login(LoginViewModel loginViewModel)
        {
            if (ModelState.IsValid)
            {
                AccountManager loginManager = new AccountManager();

                if (loginManager.ValidatedUser(loginViewModel.Email, loginViewModel.Password))
                {
                    FormsAuthentication.SetAuthCookie(loginViewModel.Email, false);
                    return RedirectToAction("Contact","Home");
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
