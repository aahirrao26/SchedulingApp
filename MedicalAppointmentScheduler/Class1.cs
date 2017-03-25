using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Mvc.Ajax;

namespace MvcApplication1.Controllers
{
    public class AdminController : Controller
    {
        //
        // GET: /Products/

        public ActionResult Index()
        {
            // Add action logic here to load AddUser view
            return View();
        }
    }
}
