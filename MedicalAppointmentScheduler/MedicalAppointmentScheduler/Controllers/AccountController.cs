using System.Web.Mvc;
using MedicalAppointmentScheduler.Core.Business;
using MedicalAppointmentScheduler.Core.Data;

namespace MedicalAppointmentScheduler.Controllers
{
    public class AccountController : Controller
    {      
        IAccountManager loginManager;
        IAuthentication authenticationHelper;

        public AccountController()
        {            
            loginManager = new AccountManager();
            authenticationHelper = new FormsAuth();

        }

        public AccountController(IAccountManager _loginManager, IAuthentication _authenticationHelper)
        {           
            loginManager = _loginManager;
            authenticationHelper = _authenticationHelper;
        }

        //GET: this action is called for all anonymous users to get authenticated
       [AllowAnonymous]
        public ActionResult Login(string returnUrl)
        {
            if (User.Identity.IsAuthenticated)
            {
                string controllerRole = Session["LoggedInUserRole"].ToString();
                return RedirectToAction("Index",controllerRole);
            }

            ViewBag.ReturnUrl = returnUrl;
            return View();
        }

        //POST: Authenticate the user and redirect to action based on its role
        [HttpPost]
        public ActionResult Login(UserLogin loginViewModel)
        {
               if (ModelState.IsValid)
                {
                    int userId = loginManager.ValidateUser(loginViewModel.Email, loginViewModel.Password);
                    if (userId != 0)
                    {
                        authenticationHelper.SetAuthCookie(loginViewModel.Email);
                        Session.Add("LoggedInUser", userId);
                        string controllerRole = loginManager.GetUserRole(userId) == null ? "Home" : loginManager.GetUserRole(userId);
                        Session.Add("LoggedInUserRole", controllerRole);
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
            authenticationHelper.LogOff();
            return RedirectToAction("Index", "Home");
        }

    }
}
