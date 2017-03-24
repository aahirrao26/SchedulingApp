using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using MedicalAppointmentScheduler.Core.Data;

namespace MedicalAppointmentScheduler.Controllers
{
    [Authorize]
    public class AdministratorController : Controller
    {
        private MedicalSchedulerDBEntities db = new MedicalSchedulerDBEntities();

        // GET: Administrator
        public ActionResult Index()
        {
            var userDetails = db.UserDetails.Include(u => u.L_User_Roles);
            return View(userDetails.ToList());
        }

        // GET: Administrator/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            UserDetails userDetails = db.UserDetails.Find(id);
            if (userDetails == null)
            {
                return HttpNotFound();
            }
            return View(userDetails);
        }

        // GET: Administrator/Create
        public ActionResult Create()
        {
            ViewBag.RoleID = new SelectList(db.UserRoles, "ID", "RoleName");
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ID,FirstName,LastName,Phone,EmailAdress,RoleID")] UserDetails userDetails)
        {
            if (ModelState.IsValid)
            {
                db.UserDetails.Add(userDetails);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.RoleID = new SelectList(db.UserRoles, "ID", "RoleName", userDetails.RoleID);
            return View(userDetails);
        }

        // GET: Administrator/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            UserDetails userDetails = db.UserDetails.Find(id);
            if (userDetails == null)
            {
                return HttpNotFound();
            }
            ViewBag.RoleID = new SelectList(db.UserRoles, "ID", "RoleName", userDetails.RoleID);
            return View(userDetails);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ID,FirstName,LastName,Phone,EmailAdress,RoleID")] UserDetails userDetails)
        {
            if (ModelState.IsValid)
            {
                db.Entry(userDetails).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.RoleID = new SelectList(db.UserRoles, "ID", "RoleName", userDetails.RoleID);
            return View(userDetails);
        }

        // GET: Administrator/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            UserDetails userDetails = db.UserDetails.Find(id);
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
            UserDetails userDetails = db.UserDetails.Find(id);
            db.UserDetails.Remove(userDetails);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
