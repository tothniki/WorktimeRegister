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

                        SetCompanyDetails(ws);

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
            ws.Cells["A1:B1"].Merge = true;
            ws.Cells[1, 1].Value = "Worktime sheet " + Year + "-" + Month;
        }

        private void SetUserDetails(ExcelWorksheet ws, UserProfile user, string fullName)
        {
            ws.Cells[3, 1].Value = fullName;
            ws.Cells[4, 1].Value = "Date of Birth:";
            ws.Cells[4, 2].Value = user.DateOfBirth;
            ws.Cells[5, 1].Value = "Phone number:";
            ws.Cells[5, 2].Value = user.PhoneNumber;
            ws.Cells["A6:B6"].Merge = true;
            ws.Cells[6, 1].Value = "Email: " + user.Email;
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

                            DateTime arrive = (DateTime)worktime.Arrival;
                            DateTime leaving = (DateTime)worktime.Leaving;
                            TimeSpan duration = leaving-arrive;
                            hoursADay = duration.TotalSeconds / 3600.0;
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
            ws.Cells[row +1, 3].Value = "Sum hours:";
            ws.Cells[row +1, 4].Value = sumHoursADay;
        }

        private void FormatWorksheet(ExcelWorksheet ws, int row)
        {
            //Format Page Header
            using (ExcelRange rng = ws.Cells[ "A1:E1"])
            {
                rng.Style.Font.Bold = true;
                rng.Style.Fill.PatternType = ExcelFillStyle.Solid; //Set Pattern for the background to Solid 
                rng.Style.Fill.BackgroundColor.SetColor(Color.LightSlateGray); //Set color to DarkGray 
                rng.Style.Font.Color.SetColor(Color.Black);
            }

            //
            // Format User infos
            using (ExcelRange rng = ws.Cells["A3:B6"])
            {
                rng.Style.Font.Bold = true;
                rng.Style.Fill.PatternType = ExcelFillStyle.Solid; //Set Pattern for the background to Solid 
                rng.Style.Fill.BackgroundColor.SetColor(Color.LightSlateGray); //Set color to DarkGray 
                rng.Style.Font.Color.SetColor(Color.Black);
                rng.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
            }

            //Format Date Of Birth
            using (ExcelRange cell = ws.Cells[4, 2])
            {
                cell.Style.Numberformat.Format = "yyyy.mm.dd";
            }

            //
            // Format Company infos
            using (ExcelRange rng = ws.Cells["D3:E6"])
            {
                rng.Style.Font.Bold = true;
                rng.Style.Fill.PatternType = ExcelFillStyle.Solid; //Set Pattern for the background to Solid 
                rng.Style.Fill.BackgroundColor.SetColor(Color.LightSlateGray); //Set color to DarkGray 
                rng.Style.Font.Color.SetColor(Color.Black);
                rng.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
            }

            //Format table header
            using (ExcelRange rng = ws.Cells["A8:D8"])
            {
                rng.Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
                rng.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;

            }

            //Format table border part
            using (ExcelRange rng = ws.Cells["B8:C8"])
            {
                rng.Style.Border.Left.Style = ExcelBorderStyle.Thin;
                rng.Style.Border.Right.Style = ExcelBorderStyle.Thin;
            }

            //Format Date col
            using (ExcelRange rng = ws.Cells[9, 1, row - 1, 1])
            {
                rng.Style.Numberformat.Format = "yyyy.mm.dd";
                rng.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
            }

            
            //Format Arrival, Leaving cols
            using (ExcelRange rng = ws.Cells[9, 2, row - 1, 3])
            {
                rng.Style.Numberformat.Format = "hh:mm:ss";
                rng.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                rng.Style.Border.Left.Style = ExcelBorderStyle.Thin;
                rng.Style.Border.Right.Style = ExcelBorderStyle.Thin;

            }

            //format Sum hours:
            using (ExcelRange rng = ws.Cells[row+1, 3, row+1 , 4])
            {
                rng.Style.Numberformat.Format = "0.00";
                rng.Style.Border.BorderAround(ExcelBorderStyle.Thin);
                rng.Style.Border.Left.Style = ExcelBorderStyle.Thin;
                rng.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
            }

            //format the Hours col
            using (ExcelRange rng = ws.Cells[9, 4, row - 1, 4])
            {
                rng.Style.Numberformat.Format = "0.00";
            }

            //Set autofit the cells' size
            ws.Cells.AutoFitColumns();
        }

        private void SetCompanyDetails(ExcelWorksheet ws)
        {
            Company company = _db.Company.FirstOrDefault();

            ws.Cells[3, 4].Value = company.Name;
            ws.Cells[4, 4].Value = "Address:";
            ws.Cells[4, 5].Value = company.Country + ", " + company.City;
            ws.Cells[5, 5].Value = company.PostCode + ", " + company.Address;
            ws.Cells["D6:E6"].Merge = true;
            ws.Cells[6, 4].Value = "Email: " + company.Email;
        }
    }
}