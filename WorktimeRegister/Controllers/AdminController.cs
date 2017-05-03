using OfficeOpenXml;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using WebMatrix.WebData;
using WorktimeRegister.Classes;
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
        // GET: /Admin/SearchWorktime

        public ActionResult SearchWorktime(int? userId = null, int? searchYear = null, int? searchMonth = null, int? searchDay = null)
        {
            ICollection<Worktimes> worktimeList;

            if (userId != null)
            {
                //Get the userprofile
                UserProfile user = _db.UserProfiles.First(u => u.UserId == userId);
                worktimeList = user.Worktimes.ToList();
            }
            else
            {
                //With .ToList make de db selection to ICollection<> type
                worktimeList = _db.Worktimes.Select(r => r).ToList();
            }

            var worktimeLBD = new WorktimeListByDate(worktimeList, searchYear, searchMonth, searchDay);
            var model = worktimeLBD.getWorktimeList();

            return View(model);
        }

        //Worktimes' modificaton actions
        //------------------------------------------------------------------------

        //
        // GET: /Admin/EditUserWorktime

        public ActionResult EditUserWorktime(int id)
        {
            Worktimes worktime = _db.Worktimes.First(u => u.Id == id);
            return View(worktime);
        }

        //
        // POST: /Admin/EditUserWorktime

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EditUserWorktime(int id, Worktimes worktime)
        {
            if (ModelState.IsValid)
            {
                _db.Entry(worktime).State = System.Data.EntityState.Modified;
                _db.SaveChanges();
                return RedirectToAction("SearchWorktime", new { userId = worktime.UserId });
            }

            return View(worktime);
        }

        //
        // GET: /Admin/DeleteUserWorktime

        public ActionResult DeleteUserWorktime(int id)
        {
            Worktimes worktime = _db.Worktimes.First(u => u.Id == id);
            return View(worktime);
        }

        //
        // POST: /Admin/DeleteUserWorktime

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteUserWorktime(int id, Worktimes worktime)
        {
            var worktimeList = _db.Worktimes.Where(u => u.Id == id).Select(u => u);
            if (worktimeList.Any())
            {
                var delWorktime = worktimeList.First();
                _db.Worktimes.Remove(delWorktime);
                _db.SaveChanges();
                return RedirectToAction("SearchWorktime", new { userId = delWorktime.UserId });
            }

            //Ha kap Id-t, de nemtalál elemet hozzá a DB-ben?
            return View(worktime);
        }

        //
        // GET: /Admin/CreateUserWorktime

        public ActionResult CreateUserWorktime(int userId)
        {
            return View();
        }

        //
        // POST: /Admin/CreateUserWorktime

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CreateUserWorktime(int userId, Worktimes worktime)
        {
            //Get the userprofile
            UserProfile user = _db.UserProfiles.First(u => u.UserId == userId);
            Worktimes newWorktime = new Worktimes();

            if (ModelState.IsValid)
            {
                DateTime arrive = new DateTime(worktime.Date.Year, worktime.Date.Month, worktime.Date.Day, worktime.Arrival.Value.Hour, worktime.Arrival.Value.Minute, worktime.Arrival.Value.Second);
                DateTime leaving = new DateTime(worktime.Date.Year, worktime.Date.Month, worktime.Date.Day, worktime.Leaving.Value.Hour, worktime.Leaving.Value.Minute, worktime.Leaving.Value.Second);

                newWorktime.Date = worktime.Date;
                newWorktime.Arrival = arrive;
                newWorktime.Leaving = leaving;
                newWorktime.UserId = worktime.UserId;
                _db.Worktimes.Add(newWorktime);
                _db.SaveChanges();
                return RedirectToAction("SearchWorktime", new { userId = newWorktime.UserId });
            }
            return View(newWorktime);
        }

        //UserProfiles' modificaton actions
        //------------------------------------------------------------------------

        //
        // GET: /Admin/Users

        public ActionResult Users()
        {
            //Exception: There is already an open DataReader associated with this Command which must be closed first
            //Solution: if you dont "force" execution of the select by "enumerating" query by ToList, it is in fact executed too late - in view.
            var users = _db.UserProfiles.OrderBy(r => r.UserName)
                                .Select(r => r).ToList();

            return View(users);

        }


        //
        // GET: /Admin/EditUserInfo

        public ActionResult EditUserInfo(int id)
        {
            //Get the userprofile
            UserProfile currentUserProfileModel = _db.UserProfiles.First(u => u.UserId == id);

            return View("~/Views/Account/ManageUserInfo.cshtml", currentUserProfileModel);
        }

        //
        // POST: /Admin/EditUserInfo

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EditUserInfo(int id, UserProfile currentUserProfileModel)
        {
            if (ModelState.IsValid)
            {
                //Get the userprofile
                UserProfile user = _db.UserProfiles.First(u => u.UserId == id);

                //Update fields
                user.FirstName = currentUserProfileModel.FirstName;
                user.LastName = currentUserProfileModel.LastName;
                user.Email = currentUserProfileModel.Email;
                user.PhoneNumber = currentUserProfileModel.PhoneNumber;

                _db.Entry(user).State = System.Data.EntityState.Modified;
                _db.SaveChanges();

                return RedirectToAction("Users", "Admin");
            }

            // If we got this far, something failed, redisplay form
            return View("~/Views/Account/ManageUserInfo.cshtml", currentUserProfileModel);
        }


        //
        // GET: /Admin/DeleteUser/5

        public ActionResult DeleteUser(int id)
        {
            var userList = _db.UserProfiles.Where(u => u.UserId == id).Take(1);
            if (userList.Any())
            {
                var user = userList.First();
                return View(user);
            }
            return View("Index");
        }


        //
        // POST: /Admin/DeleteUser

        [HttpPost]
        public ActionResult DeleteUser(UserProfile userProfile)
        {
            var roles = (SimpleRoleProvider)Roles.Provider;
            var membership = (SimpleMembershipProvider)Membership.Provider;

            if (!roles.GetRolesForUser(userProfile.UserName).Any())
            {
                bool deletedAcc = membership.DeleteAccount(userProfile.UserName);
                bool deletedUser = false;
                if (deletedAcc)
                {
                    deletedUser = membership.DeleteUser(userProfile.UserName, true);
                    //_db.UserProfiles.Remove(user);
                    _db.SaveChanges();
                    return RedirectToAction("Users", "Admin");
                }
                else if (!deletedAcc || !deletedUser)
                {
                    //Kéne valami error page !!!!!!!!!
                    return RedirectToAction("Users", "Admin");
                }
            }
            else if (roles.GetRolesForUser(userProfile.UserName).Contains("Admin"))
            {
                //Valami page hogy admint nem törölhet
                return RedirectToAction("Users", "Admin");
            }

            //Kéne valami error page !!!!!!!!!
            return RedirectToAction("Users", "Admin");
        }

        //Excel generator actions
        //---------------------------------------------------------
        //Admin/Export

        public ActionResult Export()
        {
            var worktimeList = _db.Worktimes.OrderBy(r => r.Date.Year).ToList();
            if (worktimeList.Any())
            {
                var date = worktimeList.FirstOrDefault();
                return View(date.Date);
            }
            else
            {
                return View(DateTime.Now);
            }
        }

        [HttpPost]
        public FileContentResult Export(FormCollection form)
        {
            var year = Request.Form["DropDownYear"].ToString();
            var month = Request.Form["DropDownMonth"].ToString();
            var fileDownloadName = String.Format("Export_Worktime_" + year + "_" + month + ".xlsx");
            const string contentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";

            CustomExcelHelper excelhelper = new CustomExcelHelper(year, month, _db);
            // Pass your ef data to method
            ExcelPackage package = excelhelper.GenerateExcelFile();

            var fsr = new FileContentResult(package.GetAsByteArray(), contentType);
            fsr.FileDownloadName = fileDownloadName;

            return fsr;
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
