using SeeNowProcess.Models;
using SeeNowProcess.Models.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SeeNowProcess.DAL
{
    public class SeeNowInitializer : System.Data.Entity.DropCreateDatabaseIfModelChanges<SeeNowContext>
        {
        protected void AddUsers(SeeNowContext context)
        {

            var Users = new List<User>
            {
                new User {Login="asdfg", Email="As.Dfg@company.com", Name="As Dfg", Password="admin11", PhoneNumber="111-222-333", role=Role.Admin },
                new User {Login="qwert", Email="Qw.Ert@company.com", Name="Qw Ert", Password="secret1", PhoneNumber="123-456-789", role=Role.HeadMaster },
                new User {Login="kajak", Email="Kaj.Kajak@company.com",Name="Kaj Kajak", Password="parostatkiem", PhoneNumber="555-666-777", role=Role.SeniorDev },
                new User {Login="adada", Email="Adam.Adamiak@company.com", Name="Adam Adamiak", Password="pieknyRejs", PhoneNumber="356-865-345", role=Role.JuniorDev }
            };
            Users[0].Supervisor = Users[1];
            Users[2].Supervisor = Users[1];
            Users[3].Supervisor = Users[2];

            Users.ForEach(u => context.Users.Add(u));
            context.SaveChanges();
        }
        void AddTeams(SeeNowContext context)
        {

        }
        protected override void Seed(SeeNowContext context)
        {
            AddProjects(context);
            AddUsers(context);
            AddProblems(context);
            AddTeams(context);
            AddUserStories(context);
            AddAssignments(context);
            AddBoxes(context);

        }

        private void AddBoxes(SeeNowContext context)
        {
            throw new NotImplementedException();
        }

        private void AddAssignments(SeeNowContext context)
        {
            throw new NotImplementedException();
        }

        private void AddUserStories(SeeNowContext context)
        {
            throw new NotImplementedException();
        }

        private void AddProjects(SeeNowContext context)
        {
            // na razie tylko dwa projekty z czego jeden będzie pusty (żeby był jakiś wybór)
            List<Project> Projects = new List<Project>
            {
                new Project {Name="First Project", Description="Our company's first project. It must be the best!", StartDate=new DateTime(2017, 4, 1, 3, 0, 0), Status=Status.Open },
                new Project {Name="Next Project", Description="Our government asked us to create some software for them.", StartDate=new DateTime(2017,1,1,0,0,0), Status=Status.Suspended}
            };
            Projects.ForEach(p => context.Projects.Add(p));
            context.SaveChanges();
        }

        private void AddProblems(SeeNowContext context)
        {
            throw new NotImplementedException();
        }
    }
}