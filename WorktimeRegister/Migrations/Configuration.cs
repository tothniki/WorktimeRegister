namespace WorktimeRegister.Migrations
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;
    using System.Web.Security;
    using WebMatrix.WebData;
    using WorktimeRegister.Models;

    internal sealed class Configuration : DbMigrationsConfiguration<WorktimeRegister.Models.WorktimeRegisterDb>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = true;
        }

        protected override void Seed(WorktimeRegister.Models.WorktimeRegisterDb context)
        {
            //Looking them up by Name
            context.Worktimes.AddOrUpdate(r => r.Name,
                 new Worktimes
                 {
                     Id = 1,
                     Name = "Niki",
                     Date = DateTime.Today.AddDays(2),
                     Arrival = DateTime.Now.AddHours(2),
                     Leaving = DateTime.Now.AddHours(8),
                     Email = "tothniki.ntt@gmail.com"
                 },
             new Worktimes
             {
                 Id = 2,
                 Name = "Dani",
                 Date = DateTime.Today.AddDays(4),
                 Arrival = DateTime.Now.AddHours(3),
                 Leaving = DateTime.Now.AddHours(5),
                 Email= "fafula.dani@gmail.com"
             },
             new Worktimes
             {
                 Id = 3,
                 Name = "Gonzi",
                 Date = DateTime.Today,
                 Arrival = DateTime.Now,
                 Leaving = DateTime.Now.AddHours(6),
                 Email = "nagy.gonzi@gmail.com"
             });

            //Seed user and admin
            SeedMembership();
        }

        private void SeedMembership()
        {
            //just to make sure that everything is set up
            WebSecurity.InitializeDatabaseConnection("DefaultConnection", "UserProfile", "UserId", "UserName", autoCreateTables: true);
            
            //get access to a current role provider
            var roles = (SimpleRoleProvider)Roles.Provider;
            var membership = (SimpleMembershipProvider)Membership.Provider;

            if (!roles.RoleExists("Admin"))
            {
                roles.CreateRole("Admin");
            }
            if(membership.GetUser("adminUser", false) == null)
            {
                membership.CreateUserAndAccount("adminUser", "adminUser123");
            }
            if (!roles.GetRolesForUser("adminUser").Contains("Admin"))
            {
                roles.AddUsersToRoles(new[] { "adminUser" }, new[] { "Admin" });
            }
        }
    }
}
