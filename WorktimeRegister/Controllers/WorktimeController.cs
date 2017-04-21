﻿using System;
using System.Collections.Generic;
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

        [Authorize(Roles = "Admin")] //this is just an example
        public ActionResult Index(int? searchYear= null, int? searchMonth = null, int? searchDay = null)
        {
            var worktimeLBD = new WorktimeListByDate(searchYear, searchMonth, searchDay);
            var model = worktimeLBD.getWorktimeList();
            return View(model);
        }
        

        //
        // GET: /Worktime/Details/5

        public ActionResult Details(int id)
        {
            return View();
        }

        //
        // GET: /Worktime/Create

        public ActionResult Create()
        {
            return View();
        }

        //
        // POST: /Worktime/Create

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
            return View();
           
        }

        ////
        //// GET: /Worktime/Edit/5
        ////For Admin user
        //public ActionResult Edit(int id)
        //{
        //    return View();
        //}

        ////
        //// POST: /Worktime/Edit/5
        ////for admin user
        //[HttpPost]
        //public ActionResult Edit(int id, FormCollection collection)
        //{
        //    try
        //    {
        //        // TODO: Add update logic here

        //        return RedirectToAction("Index");
        //    }
        //    catch
        //    {
        //        return View();
        //    }
        //}

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
