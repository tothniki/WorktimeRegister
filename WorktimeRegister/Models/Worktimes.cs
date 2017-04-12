using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WorktimeRegister.Models
{
    public class Worktimes
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public DateTime Date { get; set; }
        public DateTime Arrival { get; set; }
        public DateTime Leaving { get; set; }
        public int UserId { get; set; }
    }
}