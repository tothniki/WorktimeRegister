namespace WorktimeRegister.Migrations
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;
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
                     Date = DateTime.Today,
                     Arrival = DateTime.Now,
                     Leaving = DateTime.Now.AddHours(8)
                 },
             new Worktimes
             {
                 Id = 2,
                 Name = "Dani",
                 Date = DateTime.Today,
                 Arrival = DateTime.Now,
                 Leaving = DateTime.Now.AddHours(5)
             },
             new Worktimes
             {
                 Id = 3,
                 Name = "Gonzi",
                 Date = DateTime.Today,
                 Arrival = DateTime.Now,
                 Leaving = DateTime.Now.AddHours(6)
             });
        }
    }
}
