using System;
using System.Collections.Generic;
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

        public ActionResult SearchWorktime(int? searchYear = null, int? searchMonth = null, int? searchDay = null)
        {
            ICollection<Worktimes> worktimeList;
            //With .ToList make de db selection to ICollection<> type
            worktimeList = _db.Worktimes.Select(r => r).ToList();

            var worktimeLBD = new WorktimeListByDate(worktimeList, searchYear, searchMonth, searchDay);
            var model = worktimeLBD.getWorktimeList();

            return View(model);
        }

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
        // GET: /Admin/EditUserInfo

        public ActionResult EditUserInfo(int id)
        {
            //Get the userprofile
            UserProfile currentUserProfileModel = _db.UserProfiles.First(u => u.UserId.Equals(id));

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
                UserProfile user = _db.UserProfiles.First(u => u.UserId.Equals(id));

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
        // GET: /Admin/Delete/5

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
        // POST: /Admin/Delete

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
