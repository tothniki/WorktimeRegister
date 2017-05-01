using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WorktimeRegister.Models;

namespace WorktimeRegister.Controllers
{
    public class HomeController : Controller
    {
        WorktimeRegisterDb _db = new WorktimeRegisterDb();

        public ActionResult Index()
        {
            var model = from r in _db.Worktimes
                        orderby r.Date ascending
                        where r.Date == DateTime.Today
                        select r;
            return View(model.ToList());
        }

        public ActionResult About()
        {
            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }

        public ActionResult Company()
        {
            var model = _db.Company.FirstOrDefault();
            return View(model);
        }

        // Get
        // Home/Company
        [Authorize(Roles = "admin")]
        public ActionResult EditCompany(int id)
        {
            var model = _db.Company.FirstOrDefault( r => r.Id == id);
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles="admin")]
        public ActionResult EditCompany(int id, Company company)
        {
            if (ModelState.IsValid)
            {
                _db.Entry(company).State = System.Data.EntityState.Modified;
                _db.SaveChanges();
                return RedirectToAction("Company");
            }
            return View(company);
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
