using System.Web.Mvc;
using System.Web.Security;
using MedicalAppointmentScheduler.Core.Business;
using MedicalAppointmentScheduler.Core.Data;

namespace MedicalAppointmentScheduler.Controllers
{
    public class AccountController : Controller
    {
        MedicalSchedulerDBEntities dbContext;
        public AccountController() {
            dbContext = new MedicalSchedulerDBEntities();
        }
        public AccountController(MedicalSchedulerDBEntities _dbContext)
        {
            dbContext = _dbContext;
        }

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
                
                AccountManager loginManager = new AccountManager(dbContext);
                int userId = loginManager.ValidateUser(loginViewModel.Email, loginViewModel.Password);
                if (userId != 0)
                {
                    FormsAuthentication.SetAuthCookie(loginViewModel.Email, false);
                    string controllerRole = loginManager.GetUserRole(userId) == null ? "Home" : loginManager.GetUserRole(userId);
                    return RedirectToAction("Index", controllerRole);
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
