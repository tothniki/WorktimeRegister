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
                        select r;
            return View(model);
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your app description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
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
