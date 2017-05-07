using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace WorktimeRegister.Controllers
{
    public class ErrorController : Controller
    {
        public ViewResult Index()
        {
            ViewBag.ErrorMessage = "An error occurred while processing your request. If it's not resolved soon, please contact the administrator!";
            return View("Error");
        }
        public ViewResult NotFound()
        {
            ViewBag.Errorcode = 404;
            ViewBag.ErrorName = "Page Not Found";
            ViewBag.ErrorMessage = "The page you are looking for does not exist.";
            Response.StatusCode = 404;
            return View("Error");
        }
        public ViewResult ServerError()
        {
            ViewBag.Errorcode = 500;
            ViewBag.ErrorName = "Server Error";
            ViewBag.ErrorMessage = "The server encountered an internal error and was unable to complete your request.";
            Response.StatusCode = 500;
            return View("Error");
        }
    }
}
