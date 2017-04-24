using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices;
using System.Web;
using System.Web.Mvc;
using WorktimeRegister.Classes;
using WorktimeRegister.Models;

namespace WorktimeRegister.Controllers
{
    public class WorktimeController : Controller
    {
        WorktimeRegisterDb _db = new WorktimeRegisterDb();

        //[Authorize(Roles = "Admin")] //this is just an example
        public ActionResult Index(int? searchYear= null, int? searchMonth = null, int? searchDay = null)
        {
            var worktimeLBD = new WorktimeListByDate(searchYear, searchMonth, searchDay);
            var model = worktimeLBD.getWorktimeList();
            return View(model);
        }

        //
        // GET: /Worktime/SetWorktime

        public ActionResult SetWorktime()
        {
            Worktimes worktime = new Worktimes();
            var worktimeList = _db.Worktimes.OrderByDescending(r => r.Date)
                            .Where(r => r.Name == User.Identity.Name && r.Date == DateTime.Today && r.Arrival != null && r.Leaving == null)
                          .Take(1);

           // worktime = worktimeList.Single();
            if (!worktimeList.Any())
            {
                return View("Create");
            }
            else
            {
                worktime = worktimeList.Single();
                return View("Edit", worktime);
            }
        }

        //
        // GET: /Worktime/Details/5

        public ActionResult Details(int id)
        {
            return View();
        }

        //
        // GET: /Worktime/Create
        //for users
        [HttpGet]
        public ActionResult Create()
        {
            return View();
        }

        //
        // POST: /Worktime/Create
        //for users
        [HttpPost]
        public ActionResult Create(Worktimes worktime)
        {
            if(ModelState.IsValid)
            {
                worktime.Name = User.Identity.Name;
                worktime.Date = DateTime.Today;
                worktime.Arrival = DateTime.Now;
                worktime.Leaving = null;
                _db.Worktimes.Add(worktime);
                _db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(worktime);
        }

        //
        // GET: /Worktime/Edit/5
        //For Admin user
        public ActionResult Edit()
        {
            return View();
        }

        //
        // POST: /Worktime/Edit/5
        //for admin user
        [HttpPost]
        public ActionResult Edit( Worktimes worktime)
        {
             if (ModelState.IsValid)
                        {
                            worktime.Leaving = DateTime.Now;
                            _db.Entry(worktime).State = System.Data.EntityState.Modified;
                            _db.SaveChanges();
                            return RedirectToAction("Index");
                        }
                        return View(worktime);
        }


        //
        // GET: /Worktime/Delete/5

        public ActionResult Delete(int id)
        {
            return View();
        }

        //
        // POST: /Worktime/Delete/5

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
