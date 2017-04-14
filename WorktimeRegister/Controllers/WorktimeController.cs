﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WorktimeRegister.Models;

namespace WorktimeRegister.Controllers
{
    public class WorktimeController : Controller
    {
        WorktimeRegisterDb _db = new WorktimeRegisterDb();

        //public ActionResult Index(DateTime searchTerm)
        //{
        //    var model = _db.Worktimes.OrderByDescending(r => r.Date)
        //                    .Where(r => searchTerm == r.Date)
        //                    .Take(10)
        //                    .Select(r => r);
        //    return View(model);
        //}
        [Authorize(Roles="Admin")] //this is just an example
        public ActionResult Index(string searchTerm = null)
        {
            var model = _db.Worktimes.OrderByDescending(r => r.Date)
                            .Where(r => searchTerm == null || r.Name.StartsWith(searchTerm))
                            .Take(10)
                            .Select(r => r);
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
        // GET: /Worktime/Edit/5

        public ActionResult Edit(int id)
        {
            return View();
        }

        //
        // POST: /Worktime/Edit/5

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

       
    }
}
