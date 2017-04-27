using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WorktimeRegister.Models;

//This for all users' worktimes
//namespace WorktimeRegister.Classes
//{
//    public class WorktimeListByDate
//    {
//        private int? searchYear = null;
//        private int? searchMonth = null;
//        private int? searchDay = null;
//        private WorktimeRegisterDb _db;

//        public WorktimeListByDate(int? searchYear = null, int? searchMonth = null, int? searchDay = null)
//        {
//            this.searchYear = searchYear;
//            this.searchMonth = searchMonth;
//            this.searchDay = searchDay;
//            _db = new WorktimeRegisterDb();
//        }

//        public IQueryable<Worktimes> getWorktimeList()
//        {
//            IQueryable<Worktimes> model = null;
//            //Nothing given
//            if (searchYear == null && searchMonth == null && searchDay == null)
//            {
//                model = _db.Worktimes.OrderByDescending(r => r.Date)
//                            .Where(r => null == searchDay)
//                           .Take(20)
//                          .Select(r => r);
//            }
//            else
//            {
//                //Year, month, day are given
//                if (searchYear != null && searchMonth != null && searchDay != null)
//                {
//                    model = _db.Worktimes.OrderByDescending(r => r.Date)
//                                .Where(r => r.Date.Year == searchYear && r.Date.Month == searchMonth && r.Date.Day == searchDay)
//                               .Take(20)
//                              .Select(r => r);
//                }
//                else

//                    //Year, month are given
//                    if (searchYear != null && searchMonth != null)
//                    {
//                        model = _db.Worktimes.OrderByDescending(r => r.Date)
//                                    .Where(r => r.Date.Year == searchYear && r.Date.Month == searchMonth)
//                                   .Take(20)
//                                  .Select(r => r);
//                    }
//                    else

//                        //Year, day are given
//                        if (searchYear != null  && searchDay != null)
//                        {
//                            model = _db.Worktimes.OrderByDescending(r => r.Date)
//                                         .Where(r => r.Date.Year == searchYear && r.Date.Day == searchDay)
//                                       .Take(20)
//                                      .Select(r => r);
//                        }
//                        else

//                            //Month, day are given
//                            if (searchMonth != null && searchDay != null)
//                            {
//                                model = _db.Worktimes.OrderByDescending(r => r.Date)
//                                            .Where(r => r.Date.Month == searchMonth && r.Date.Day == searchDay)
//                                           .Take(20)
//                                          .Select(r => r);
//                            }
//                            else

//                                //Year given
//                                if (searchYear != null)
//                                {
//                                    model = _db.Worktimes.OrderByDescending(r => r.Date)
//                                                 .Where(r => r.Date.Year == searchYear)
//                                               .Take(20)
//                                              .Select(r => r);
//                                }
//                                else

//                                    //Month given
//                                    if (searchMonth != null )
//                                    {
//                                        model = _db.Worktimes.OrderByDescending(r => r.Date)
//                                                    .Where(r => r.Date.Month == searchMonth)
//                                                   .Take(20)
//                                                  .Select(r => r);
//                                    }
//                                    else

//                                        //Day given
//                                        if  (searchDay != null)
//                                        {
//                                            model = _db.Worktimes.OrderByDescending(r => r.Date)
//                                                         .Where(r => r.Date.Day == searchDay)
//                                                       .Take(20)
//                                                      .Select(r => r);
//                                        }
//            }

//            return model;
//        }

//        //Dispose?
//    }
//}


//This for the authenticated user's worktimes


namespace WorktimeRegister.Classes
{
    public class WorktimeListByDate
    {
        private int? searchYear = null;
        private int? searchMonth = null;
        private int? searchDay = null;
        private ICollection<Worktimes> worktimeList;

        public WorktimeListByDate( ICollection<Worktimes> worktimeList, int? searchYear = null, int? searchMonth = null, int? searchDay = null)
        {
            this.searchYear = searchYear;
            this.searchMonth = searchMonth;
            this.searchDay = searchDay;
            this.worktimeList = worktimeList;
        }

        public ICollection<Worktimes> getWorktimeList()
        {
            ICollection<Worktimes> model = null;
            //Nothing given
            if (searchYear == null && searchMonth == null && searchDay == null)
            {
                model = worktimeList;
            }
            else
            {
                //Year, month, day are given
                if (searchYear != null && searchMonth != null && searchDay != null)
                {
                    model = worktimeList.OrderByDescending(r => r.Date)
                                .Where(r => r.Date.Year == searchYear && r.Date.Month == searchMonth && r.Date.Day == searchDay).ToList();
                }


                else

                    //Year, month are given
                    if (searchYear != null && searchMonth != null)
                    {
                        model = worktimeList.OrderByDescending(r => r.Date)
                                    .Where(r => r.Date.Year == searchYear && r.Date.Month == searchMonth)
                                   .ToList();
                    }
            
                    else

                        //Year, day are given
                        if (searchYear != null && searchDay != null)
                        {
                            model = worktimeList.OrderByDescending(r => r.Date)
                                         .Where(r => r.Date.Year == searchYear && r.Date.Day == searchDay)
                                       .ToList();
                        }
                        else

                            //Month, day are given
                            if (searchMonth != null && searchDay != null)
                            {
                                model = worktimeList.OrderByDescending(r => r.Date)
                                            .Where(r => r.Date.Month == searchMonth && r.Date.Day == searchDay)
                                           .ToList();
                            }
                            else

                                //Year given
                                if (searchYear != null)
                                {
                                    model = worktimeList.OrderByDescending(r => r.Date)
                                                 .Where(r => r.Date.Year == searchYear)
                                               .ToList();
                                }
                                else

                                    //Month given
                                    if (searchMonth != null)
                                    {
                                        model = worktimeList.OrderByDescending(r => r.Date)
                                                    .Where(r => r.Date.Month == searchMonth)
                                                   .ToList();
                                    }
                                    else

                                        //Day given
                                        if (searchDay != null)
                                        {
                                            model = worktimeList.OrderByDescending(r => r.Date)
                                                         .Where(r => r.Date.Day == searchDay)
                                                       .ToList();
                                        }
            }

            return model;
        }

        //Dispose?
    }
}