using OfficeOpenXml;
using OfficeOpenXml.Style;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Web;
using System.Web.Security;
using WorktimeRegister.Models;

namespace WorktimeRegister.Classes
{
    public class CustomExcelHelper
    {
        private string Year;
        private string Month;
        private WorktimeRegisterDb _db;
        //set Table starting row : int row
        static int row = 0;

        public CustomExcelHelper(string year, string month, WorktimeRegisterDb _db)
        {
            this.Year = year;
            this.Month = month;
            this._db = _db;
        }

        //legyen ez static v ne?
        public ExcelPackage GenerateExcelFile()
        {
            ExcelPackage pck = new ExcelPackage();

            var users = _db.UserProfiles.OrderBy(r => r.UserName)
                               .Select(r => r).ToList();

            if (users.Any())
            {
                foreach (var user in users)
                {
                    if (!Roles.GetRolesForUser(user.UserName).Contains("admin"))
                    {
                        //Create user name
                        var userFullName = user.FirstName + " " + user.LastName;
                        if (userFullName == " ")
                        {
                            userFullName = user.UserName;
                        }
                        //Create the worksheet 
                        ExcelWorksheet ws = pck.Workbook.Worksheets.Add(userFullName);
                        // Set Headers
                        SetPageHeader(ws);

                        //Set user details
                        SetUserDetails(ws, user, userFullName);

                        //Set worktime Table Headers
                        SetWorktimeTableHeader(ws);

                        //Worktime table content
                        
                        SetWorktimeTableContent(ws, user);

                        //Format the worksheet
                        FormatWorksheet(ws, row);
                    }
                }
            }
            return pck;
        }

        private void SetPageHeader(ExcelWorksheet ws)
        {
            ws.Cells[1, 1].Value = "Worktime sheet";
            ws.Cells[1, 2].Value = Year + "-" + Month;
        }

        private void SetUserDetails(ExcelWorksheet ws, UserProfile user, string fullName)
        {
            ws.Cells[3, 1].Value = fullName;
            ws.Cells[4, 1].Value = "Date of Birth:";
            ws.Cells[4, 2].Value = user.DateOfBirth;
            ws.Cells[5, 1].Value = "Phone number:";
            ws.Cells[5, 2].Value = user.PhoneNumber;
            ws.Cells[6, 1].Value = "Email:";
            ws.Cells[6, 2].Value = user.Email;
        }

        private void SetWorktimeTableHeader(ExcelWorksheet ws)
        {
            ws.Cells[8, 1].Value = "Date";
            ws.Cells[8, 2].Value = "Arrive";
            ws.Cells[8, 3].Value = "Leaving";
            ws.Cells[8, 4].Value = "Hours";
            //set starting row for the worktime content table
            row = 9;
        }

        private void SetWorktimeTableContent(ExcelWorksheet ws, UserProfile user)
        {
            double hoursADay = 0;
            double sumHoursADay = 0;
            if (user.Worktimes.Any())
            {

                foreach (var worktime in user.Worktimes.ToList())
                {
                    if (worktime.Date.Year.ToString().Equals(Year) && worktime.Date.Month.ToString().Equals(Month))
                    {
                        ws.Cells[row, 1].Value = worktime.Date;
                        ws.Cells[row, 2].Value = worktime.Arrival;
                        ws.Cells[row, 3].Value = worktime.Leaving;
                        if (worktime.Leaving != null)
                        {
                            hoursADay = worktime.Leaving.Value.TimeOfDay.Subtract(worktime.Arrival.Value.TimeOfDay).Seconds / 3600.0;
                            sumHoursADay += hoursADay;
                            ws.Cells[row, 4].Value = hoursADay;
                        }
                        else
                        {
                            ws.Cells[row, 4].Value = "";
                        }
                        row++;
                    }
                }
            }
            row += 1;
            ws.Cells[row, 3].Value = "Sum hours:";
            ws.Cells[row, 4].Value = sumHoursADay;
        }

        private void FormatWorksheet(ExcelWorksheet ws, int row)
        {
            //
            // Format the excel content
            using (ExcelRange rng = ws.Cells["A1:D6"])
            {
                rng.Style.Font.Bold = true;
                rng.Style.Fill.PatternType = ExcelFillStyle.Solid; //Set Pattern for the background to Solid 
                rng.Style.Fill.BackgroundColor.SetColor(Color.LightBlue); //Set color to DarkGray 
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
            using (ExcelRange cell = ws.Cells[4, 2])
            {
                cell.Style.Numberformat.Format = "yyyy.mm.dd";
            }

            //Set autofit the cells' size
            ws.Cells.AutoFitColumns();
        }

    }
}