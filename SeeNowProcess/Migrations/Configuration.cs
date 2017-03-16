namespace SeeNowProcess.Migrations
{
    using Models;
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;

    internal sealed class Configuration : DbMigrationsConfiguration<SeeNowProcess.Models.SimpleTaskDBContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
        }

        protected override void Seed(SeeNowProcess.Models.SimpleTaskDBContext context)
        {
            context.SimpleTasks.AddOrUpdate(i => i.Title,
                new SimpleTask
                {
                    Title = "title1",
                    CreationDate = DateTime.Parse("1989-1-11"),
                    CompletionDate = DateTime.Parse("1989-1-11"),
                    Importance = Importance.Critical,
                    CurrentState = State.Finished,
                    Description = "desc1",
                    Progress = 80,
                    Comments = "Comm1"
                },

                new SimpleTask
                {
                    Title = "title2",
                    CreationDate = DateTime.Parse("1989-1-11"),
                    CompletionDate = DateTime.Parse("1989-1-11"),
                    Importance = Importance.Regular,
                    CurrentState = State.InProgress,
                    Description = "desc2",
                    Progress = 50,
                    Comments = "Comm2"
                    
                },

                new SimpleTask
                {
                    Title = "title3",
                    CreationDate = DateTime.Parse("1989-1-11"),
                    CompletionDate = DateTime.Parse("1989-1-11"),
                    Importance = Importance.Trivial,
                    CurrentState = State.Created,
                    Description = "desc3",
                    Progress = 20,
                    Comments = "Comm3"
                }
           );

        }

    }
}
