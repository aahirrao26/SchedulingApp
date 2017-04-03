using System;
using System.Collections.Generic;
using System.Net;
using System.Web.Mvc;
using MedicalAppointmentScheduler.Core.Data;
using MedicalAppointmentScheduler.Core.Business;
using PagedList;

namespace MedicalAppointmentScheduler.Controllers
{
    [Authorize]
    [OutputCache(NoStore = true, Duration = 0)]
    public class AdministratorController : Controller
    {
        private AdminManager adminManager;

        public AdministratorController()
        {
            adminManager = new AdminManager();
        }

        public AdministratorController(AdminManager _adminManager)
        {
            adminManager = _adminManager;
        }

        // GET: Administrator
        public ActionResult Index(int? page)
        {
            int pageSize =5;
            int pageIndex = 1;
            pageIndex = page.HasValue ? Convert.ToInt32(page) : 1;

            List<UserDetails> userList = adminManager.GetUserList();
            return View(userList.ToPagedList(pageIndex, pageSize));
        }

        // GET: Administrator/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            UserDetails userDetails = adminManager.FindUser(id);
            if (userDetails == null)
            {
                return HttpNotFound();
            }
            return View(userDetails);
        }

        // GET: Administrator/Create
        public ActionResult Create()
        {
            ViewBag.RoleID = new SelectList(adminManager.GetRoles(), "ID", "RoleName");
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ID,FirstName,LastName,Phone,EmailAdress,RoleID")] UserDetails userDetails)
        {
            if (ModelState.IsValid)
            {               
                adminManager.CreateUser(userDetails);
                return RedirectToAction("Index");
            }

            ViewBag.RoleID = new SelectList(adminManager.GetRoles(), "ID", "RoleName", userDetails.RoleID);
            return View(userDetails);
        }

        // GET: Administrator/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            UserDetails userDetails = adminManager.FindUser(id);
            if (userDetails == null)
            {
                return HttpNotFound();
            }
            ViewBag.RoleID = new SelectList(adminManager.GetRoles(), "ID", "RoleName", userDetails.RoleID);
            return View(userDetails);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ID,FirstName,LastName,Phone,EmailAdress,RoleID")] UserDetails userDetails)
        {
            if (ModelState.IsValid)
            {                          
                adminManager.EditUser(userDetails);
                return RedirectToAction("Index");
            }
            ViewBag.RoleID = new SelectList(adminManager.GetRoles(), "ID", "RoleName", userDetails.RoleID);
            return View(userDetails);
        }

        // GET: Administrator/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            UserDetails userDetails = adminManager.FindUser(id);
            if (userDetails == null)
            {
                return HttpNotFound();
            }
            return View(userDetails);
        }

        // POST: Administrator/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {                    
            adminManager.DeleteUser(id);
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                adminManager.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
