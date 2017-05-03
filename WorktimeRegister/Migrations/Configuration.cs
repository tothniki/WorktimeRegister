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
            context.Company.AddOrUpdate(r => r.Id,
               new Company
               {
                    Name = "Cake Shop Garden Zrt.",
                    Country = "Hungary",
                    PostCode = 1117,
                    City = "Budapest",
                    Address = "Irinyi J. 42",
                    Email = "cakeshopgarden@gmail.com"
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
