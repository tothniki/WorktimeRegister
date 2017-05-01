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
    [Authorize]
    public class WorktimeController : Controller
    {
        WorktimeRegisterDb _db = new WorktimeRegisterDb();

        //
        // GET: /Worktime/SearchWorktime

        public ActionResult SearchWorktime(int? searchYear = null, int? searchMonth = null, int? searchDay = null)
        {
            ICollection<Worktimes> worktimeList;

            //Get current user
            string username = User.Identity.Name;
            UserProfile user = _db.UserProfiles.First(u => u.UserName.Equals(username));

            worktimeList = user.Worktimes;
            var worktimeLBD = new WorktimeListByDate(worktimeList, searchYear, searchMonth, searchDay);
            var model = worktimeLBD.getWorktimeList();

            return View(model);
        }

        //
        // GET: /Worktime/SetWorktime

        public ActionResult SetWorktime()
        {
            string username = User.Identity.Name;
            UserProfile user = _db.UserProfiles.First(u => u.UserName.Equals(username));
            var worktimes = user.Worktimes.OrderByDescending(r => r.Date).Where(r => r.Date == DateTime.Today && r.Arrival != null && r.Leaving == null);

            if (!worktimes.Any())
            {
                return View("Create");
            }
            else
            {
                var worktime = worktimes.First();
                return View("Edit", worktime);
            }
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
            string username = User.Identity.Name;
            UserProfile user = _db.UserProfiles.First(u=>u.UserName.Equals(username));

            if(ModelState.IsValid)
            {
               // worktime.Name = User.Identity.Name;
                worktime.Date = DateTime.Today;
                worktime.Arrival = DateTime.Now;
                worktime.Leaving = null;
                worktime.UserId = user.UserId;
                _db.Worktimes.Add(worktime);
                _db.SaveChanges();
                return RedirectToAction("SearchWorktime");
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
                            return RedirectToAction("SearchWorktime");
                        }
                        return View(worktime);
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
