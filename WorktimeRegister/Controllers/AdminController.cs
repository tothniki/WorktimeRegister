using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WorktimeRegister.Models;

namespace WorktimeRegister.Controllers
{
    [Authorize(Roles = "Admin")]
    public class AdminController : Controller
    {
        WorktimeRegisterDb _db = new WorktimeRegisterDb();
        //
        // GET: /Admin/

        public ActionResult Index()
        {
            return RedirectToRoute(new { controller = "Home", action = "Index" });
        }

        //
        // GET: /Admin/Worktime

        public ActionResult Worktime()
        {
            var users = _db.UserProfiles.OrderBy(r => r.UserName)
                            .Select(r => r).ToList();
            return View(users);
        }

        //
        // GET: /Admin/Users

        public ActionResult Users()
        {
            string uName = "";
            if (Request.Form["SelectUser"] != null)
            {
                uName = Request.Form["SelectUser"].ToString();
                ViewBag.User = uName;
            }
                var users = _db.UserProfiles.OrderBy(r => r.UserName)
                            .Select(r => r);
            
                return View(users);
        }

        //
        // GET: /Admin/Details/5

        public ActionResult Details(int id)
        {
            return View();
        }

        //
        // GET: /Admin/Create

        public ActionResult Create()
        {
            return View();
        }

        //
        // POST: /Admin/Create

        [HttpPost]
        public ActionResult Create(FormCollection collection)
        {
            try
            {
                // TODO: Add insert logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        //
        // GET: /Admin/Edit/5

        public ActionResult Edit(int id)
        {
            return View();
        }

        //
        // POST: /Admin/Edit/5

        [HttpPost]
        public ActionResult Edit(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add update logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        //
        // GET: /Admin/Delete/5

        public ActionResult Delete(int id)
        {
            return View();
        }

        //
        // POST: /Admin/Delete/5

        [HttpPost]
        public ActionResult Delete(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add delete logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        protected override void Dispose(bool disposing)
        {
            if (_db != null)
            {
                _db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
