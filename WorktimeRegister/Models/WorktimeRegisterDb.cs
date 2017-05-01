using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace WorktimeRegister.Models
{
    public class WorktimeRegisterDb : DbContext
    {
        public WorktimeRegisterDb() : base("name = DefaultConnection")
        {

        }
        public DbSet<Worktimes> Worktimes { get; set; }
        public DbSet<UserProfile> UserProfiles { get; set; }
        public DbSet<Company> Company { get; set; }
    }
}