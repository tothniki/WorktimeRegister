using OfficeOpenXml;
using OfficeOpenXml.Style;
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

        //
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


            // Pass your ef data to method
            ExcelPackage package = GenerateExcelFile(year, month);

            var fsr = new FileContentResult(package.GetAsByteArray(), contentType);
            fsr.FileDownloadName = fileDownloadName;

            return fsr;
        }

        //legyen ez static v ne?
        private ExcelPackage GenerateExcelFile(string year, string month)
        {
            ExcelPackage pck = new ExcelPackage();

            var users = _db.UserProfiles.OrderBy(r => r.UserName)
                               .Select(r => r).ToList();

            if (users.Any())
            {
                foreach (var user in users)
                {
                    var userFullName = user.FirstName + "_" + user.LastName;
                    if (userFullName == "_")
                    {
                        userFullName = user.UserName;
                    }
                    //Create the worksheet 
                    ExcelWorksheet ws = pck.Workbook.Worksheets.Add(userFullName);
                    // Sets Headers
                    ws.Cells[1, 1].Value = "Worktime sheet";
                    ws.Cells[1, 2].Value = year + "-" + month;

                    //User details
                    ws.Cells[3, 1].Value = userFullName;
                    ws.Cells[4, 1].Value = "Date of Birth:";
                    ws.Cells[4, 2].Value = user.DateOfBirth;
                    ws.Cells[5, 1].Value = "Phone number:";
                    ws.Cells[5, 2].Value = user.PhoneNumber;
                    ws.Cells[6, 1].Value = "Email:";
                    ws.Cells[6, 2].Value = user.Email;

                    //Worktime Table Headers
                    ws.Cells[8, 1].Value = "Date";
                    ws.Cells[8, 2].Value = "Arrive";
                    ws.Cells[8, 3].Value = "Leaving";
                    ws.Cells[8, 4].Value = "Hours";

                    //Worktime table content
                    int row = 9;
                    double hoursADay = 0;
                    double sumHoursADay = 0;
                    if (user.Worktimes.Any())
                    {
                        
                        foreach (var worktime in user.Worktimes.ToList())
                        {
                            if (worktime.Date.Year.ToString().Equals(year) && worktime.Date.Month.ToString().Equals(month))
                            {
                                ws.Cells[row,1].Value = worktime.Date;
                                ws.Cells[row,2].Value = worktime.Arrival;
                                ws.Cells[row,3].Value = worktime.Leaving;
                                if(worktime.Leaving != null){
                                    hoursADay = worktime.Leaving.Value.TimeOfDay.Subtract(worktime.Arrival.Value.TimeOfDay).Seconds / 3600.0;
                                    sumHoursADay += hoursADay;
                                    ws.Cells[row, 4].Value = hoursADay;
                                }else{
                                    ws.Cells[row, 4].Value = "";
                                }
                                row++;
                            }
                        }
                    }
                    row += 1;
                    ws.Cells[row, 3].Value = "Sum hours:";
                    ws.Cells[row, 4].Value = sumHoursADay;

                    //
                    // Format the excel content
                    using (ExcelRange rng = ws.Cells["A1:D6"])
                    {
                        rng.Style.Font.Bold = true;
                        rng.Style.Fill.PatternType = ExcelFillStyle.Solid; //Set Pattern for the background to Solid 
                        rng.Style.Fill.BackgroundColor.SetColor(Color.LightBlue ); //Set color to DarkGray 
                        rng.Style.Font.Color.SetColor(Color.Black);
                    }
                    //Format Date col
                    using (ExcelRange rng = ws.Cells[9, 1, row - 1, 1])
                    {
                        rng.Style.Numberformat.Format = "yyyy.mm.dd";
                        rng.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;

                    }
                    //Format Arrival, Leaving cols
                    using (ExcelRange rng = ws.Cells[9, 2, row - 1, 3])
                    {
                        rng.Style.Numberformat.Format = "hh:mm:ss";
                        rng.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;

                    }

                    //Format Date Of Birth
                    using(ExcelRange cell =  ws.Cells[4, 2]){
                        cell.Style.Numberformat.Format = "yyyy.mm.dd";
                    }

                    //Set autofit the cells' size
                    ws.Cells.AutoFitColumns();
                }


            }

            return pck;
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
