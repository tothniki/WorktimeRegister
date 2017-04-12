using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace WorktimeRegister.Models
{
    public class WorktimeRegisterDb : DbContext
    {
        public DbSet<Worktimes> Worktimes { get; set; }
    }
}