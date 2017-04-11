using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WorktimeRegister.Models;

namespace WorktimeRegister.Controllers
{
    public class WorktimeController : Controller
    {
        //
        // GET: /Worktime/

        public ActionResult Index()
        {
            var model = from w in _worktimes
                        orderby w.Name
                        select w;
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

        static List<Worktimes> _worktimes = new List<Worktimes>{
            new Worktimes{
                Id=1,
                Name="Niki",
                Date=DateTime.Today,
                Arrival=DateTime.Now,
                Leaving = DateTime.Now.AddHours(8)
            },
             new Worktimes{
                Id=2,
                Name="Dani",
                Date=DateTime.Today,
                Arrival=DateTime.Now,
                Leaving = DateTime.Now.AddHours(5)
            },
             new Worktimes{
                Id=3,
                Name="Gonzi",
                Date=DateTime.Today,
                Arrival=DateTime.Now,
                Leaving = DateTime.Now.AddHours(6)
            }

        };
    }
}
