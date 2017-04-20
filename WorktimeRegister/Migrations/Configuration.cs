namespace WorktimeRegister.Migrations
{
    using System;
    using System.Collections.Generic;
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
            //context.Worktimes.AddOrUpdate(r => r.Name,
            //     new Worktimes
            //     {
            //         Id = 1,
            //         Name = "Niki",
            //         Date = DateTime.Today.AddDays(2),
            //         Arrival = DateTime.Now.AddHours(2),
            //         Leaving = DateTime.Now.AddHours(8),
            //         Email = "tothniki.ntt@gmail.com"
            //     },
            // new Worktimes
            // {
            //     Id = 2,
            //     Name = "Dani",
            //     Date = DateTime.Today.AddDays(4),
            //     Arrival = DateTime.Now.AddHours(3),
            //     Leaving = DateTime.Now.AddHours(5),
            //     Email= "fafula.dani@gmail.com"
            // },
            // new Worktimes
            // {
            //     Id = 3,
            //     Name = "Gonzi",
            //     Date = DateTime.Today,
            //     Arrival = DateTime.Now,
            //     Leaving = DateTime.Now.AddHours(6),
            //     Email = "nagy.gonzi@gmail.com"
            // });

            context.UserProfiles.AddOrUpdate(r => r.UserName,
               new UserProfile
               {
                   UserName = "danika",
                   Email = "fafula.dani@gmail.com",
                   FirstName = "Danika",
                   LastName = "Fafula",
                   PhoneNumber = "204249919",
                   Worktimes = new List<Worktimes> {
                         new Worktimes
                         {
                             Name = "danika",
                             Date = DateTime.Today.AddYears(1),
                             Arrival = DateTime.Now.AddHours(2),
                             Leaving = DateTime.Now.AddHours(8)
                         },
                         new Worktimes
                         {
                             Name = "danika",
                             Date = DateTime.Today.AddDays(4).AddMonths(2).AddYears(2),
                             Arrival = DateTime.Now.AddHours(3),
                             Leaving = DateTime.Now.AddHours(5)
                         },
                         new Worktimes
                         {
                             Name = "danika",
                             Date = DateTime.Today.AddMonths(1),
                             Arrival = DateTime.Now,
                             Leaving = DateTime.Now.AddHours(6)
                         }
                   }
               }
           );


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
            if (membership.GetUser("adminUser", false) == null)
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
