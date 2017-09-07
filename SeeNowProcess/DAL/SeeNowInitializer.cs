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
        protected override void Seed(SeeNowContext context)
        {
            string method = "";
            try
            {
                method = "AddProjects";
                AddProjects(context);
                method = "AddIterations";
                AddIterations(context);
                method = "AddUsers";
                AddUsers(context);
                method = "AddBoxes";
                AddBoxes(context);
                method = "AddUserStories";
                AddUserStories(context);
                method = "AddTeams";
                AddTeams(context);
                method = "AddAssignments";
                AddAssignments(context);
                method = "AddProblems";
                AddProblems(context);
                method = "AssignUsersToProblems";
                AssignUsersToProblems(context);
            }
            catch (Exception e)
            {
                throw new Exception("Error in method " + method + ":\n" + e.Message);
            }
        }
        protected void AddUsers(SeeNowContext context)
        {
            var Users = new List<User>
            {
                new User {Login="asdfg", Email="As.Dfg@company.com", Name="As Dfg", Password="admin11", PhoneNumber="111-222-333", role=Role.Admin },
                new User {Login="qwert", Email="Qw.Ert@company.com", Name="Qw Ert", Password="secret1", PhoneNumber="123-456-789", role=Role.HeadMaster },
                new User {Login="kajak", Email="Kaj.Kajak@company.com",Name="Kaj Kajak", Password="parostatkiem", PhoneNumber="555-666-777", role=Role.SeniorDev },
                new User {Login="adada", Email="Adam.Adamiak@company.com", Name="Adam Adamiak", Password="pieknyRejs", PhoneNumber="356-865-345", role=Role.JuniorDev },
                new User {Login="alada", Email="Alojzy.Adamiak@company.com", Name="Alojzy Adamiak", Password="chcialbymByc", PhoneNumber="356-865-346", role=Role.JuniorDev },
                new User {Login="aliada",Email="Alicja.Adamiak@company.com", Name="Alicja Adamiak", Password="!!Marynarzem@", PhoneNumber="356-865-350", role=Role.Intern },
                new User {Login="client", Email="Nasty.Client@doitnow.com", Name="Nasty Client", Password="I'dDoItBetter", PhoneNumber="666-666-666", role=Role.Client}
            };
            Users[0].Supervisor = Users[1];
            Users[2].Supervisor = Users[1];
            Users[3].Supervisor = Users[2];
            Users[4].Supervisor = Users[2];
            Users[5].Supervisor = Users[3];

            Users.ForEach(u => context.Users.Add(u));
            context.SaveChanges();
        }
        void AddTeams(SeeNowContext context)
        {
            UserStory userStory = context.UserStories.Where(us => us.Title == "Once upon a Time").First();
            var Teams = new List<Team>
            {
                new Team {Name="Tesla Team", TeamLeader=context.Users.Where(u => u.Login=="kajak").First(), UserStories=new List<UserStory> { userStory } },
                new Team {Name="Schmetterling!", TeamLeader=context.Users.Where(u => u.Login=="adada").First(), UserStories=new List<UserStory> { userStory } }
            };
            Teams.ForEach(t => context.Teams.Add(t));
            context.SaveChanges();
        }

        public override void InitializeDatabase(SeeNowContext context)
        {
            base.InitializeDatabase(context);
            Seed(context);
        }

        private void AddBoxes(SeeNowContext context)
        {
            List<Project> Projects = context.Projects.ToList();
            List<string> BoxNames = new List<string> { "New", "Assigned", "In Progress", "Waiting For Tests", "Under Tests", "Done", "Approved" };
            foreach (Project project in Projects)
            {
                int order = 0;
                foreach (String boxName in BoxNames)
                    context.Boxes.Add(new Box { Name = boxName, Project = project, Order = order++ });
            }
            context.SaveChanges();
        }

        private void AddAssignments(SeeNowContext context)
        {
            Team teslaTeam = context.Teams.Where(t => t.Name == "Tesla Team").First();
            List<string> teslaLogins = new List<string> { "kajak", "adada" };
            List<User> teslaUsers = context.Users.Where(u => teslaLogins.Contains(u.Login)).ToList();
            Team germanTeam = context.Teams.Where(t => t.Name == "Schmetterling!").First();
            List<string> germanLogins = new List<string> { "adada", "alada", "aliada" };
            List<User> germanUsers = context.Users.Where(u => germanLogins.Contains(u.Login)).ToList();
            teslaUsers.ForEach(u => context.Assignments.Add(new Assignment { Team = teslaTeam, User = u }));
            germanUsers.ForEach(u => context.Assignments.Add(new Assignment { Team = germanTeam, User = u }));

            context.SaveChanges();

        }

        private void AddUserStories(SeeNowContext context)
        {
            Project FirstProject = context.Projects.Where(p => p.Name == "First Project").First();
            Project NewProject = context.Projects.Where(p => p.Name == "Next Project").First();
            Project RandomAccessMemoriesProject = context.Projects.Where(p => p.Name == "Random Access Memories Project").First();
            Project InterstellaProject = context.Projects.Where(p => p.Name == "Interstella Project").First();
            User Client = context.Users.Where(u => u.role == Role.Client).First();
            var UserStories = new List<UserStory>
            {
                new UserStory {Title="UserStory1", Unit="pt.", Size=12345, Owner=Client, Notes="simple note", Description="Our first User Story! Praise the Lord!!!", Criteria="I'm not sure what I want, but I want you to do it.", Project=FirstProject},
                new UserStory {Title="Once upon a Time", Unit="USD", Size=10000, Owner=Client, Notes="Try to be gentle", Description="Have no idea! Write something smart...", Project=FirstProject, Criteria="whatever..."},

                new UserStory {Title="NextUserStory", Unit="pt.", Size=12345, Owner=Client, Notes="simple note", Description="Our first User Story! Praise the Lord!!!", Criteria="I'm not sure what I want, but I want you to do it.", Project=NewProject},
                new UserStory {Title="Twice upon a Time", Unit="USD", Size=10000, Owner=Client, Notes="Try to be gentle", Description="Have no idea! Write something smart...", Project=NewProject, Criteria="whatever..."},

                new UserStory {Title="Daft story", Unit="pt.", Size=145, Owner=Client, Notes="I want it loud", Description="That's gonna be interesting", Criteria="Meke it digital and sci-fi. You know...", Project=RandomAccessMemoriesProject},

                new UserStory {Title="Punk story", Unit="USD", Size=100, Owner=Client, Notes="I dont want to live on this planet anymore", Description="There is still hope we will stay the same", Project=InterstellaProject, Criteria="It must be blue, blue to the bone."}


            };
            UserStories.ForEach(us => context.UserStories.Add(us));
            context.SaveChanges();
        }

        private void AddProjects(SeeNowContext context)
        {
            // na razie tylko dwa projekty z czego jeden będzie pusty (żeby był jakiś wybór)
            List<Project> Projects = new List<Project>
            {
                new Project {Name="First Project", Description="Our company's first project. It must be the best!", StartDate=new DateTime(2017, 4, 1, 3, 0, 0), Status=Status.Open },
                new Project {Name="Random Access Memories Project", Description="Our new cd's gonna blow your mind", StartDate=new DateTime(2013, 6, 1, 3, 0, 0), Status=Status.Closed },
                new Project {Name="Interstella Project", Description="That project should interest Hollywood", StartDate=new DateTime(2019, 4, 1, 3, 0, 0), Status=Status.Open },
                new Project {Name="Next Project", Description="Our government asked us to create some software for them.", StartDate=new DateTime(2017,1,1,0,0,0), Status=Status.Suspended}
            };
            Projects.ForEach(p => context.Projects.Add(p));
            context.SaveChanges();
        }


        private void AddProblems(SeeNowContext context)
        {
            Project FirstProject = context.Projects.Where(p => p.Name == "First Project").First();
            Project NewProject = context.Projects.Where(p => p.Name == "Next Project").First();
            Project RandomAccessMemoriesProject = context.Projects.Where(p => p.Name == "Random Access Memories Project").First();
            Project InterstellaProject = context.Projects.Where(p => p.Name == "Interstella Project").First();

            var firstProjectBoxes = context.Boxes.Where(b => b.Project.ProjectID == FirstProject.ProjectID);
            var newProjectBoxes = context.Boxes.Where(b => b.Project.ProjectID == NewProject.ProjectID);
            var randomAccessMemoriesProjectBoxes = context.Boxes.Where(b => b.Project.ProjectID == RandomAccessMemoriesProject.ProjectID);
            var interstellaProjectBoxes = context.Boxes.Where(b => b.Project.ProjectID == InterstellaProject.ProjectID);

            var firstProjectIteration = context.Iterations.Where(i => i.Name.Equals("Backlog") && i.Project.Name == "First Project").FirstOrDefault();
            var nextProjectIteration = context.Iterations.Where(i => i.Name.Equals("Backlog") && i.Project.Name == "Next Project").FirstOrDefault();
            var randomAccessMemoriesIteration = context.Iterations.Where(i => i.Name.Equals("Backlog") && i.Project.Name == "Random Access Memories Project").FirstOrDefault();
            var interstellaProjectIteration = context.Iterations.Where(i => i.Name.Equals("Backlog") && i.Project.Name == "Interstella Project").FirstOrDefault();


            UserStory onceUponATimeUserStory = context.UserStories.Where(us => us.Title == "Once upon a Time").First();
            UserStory nextUserStory = context.UserStories.Where(us => us.Title == "NextUserStory").First();
            UserStory daftStoryUserStory = context.UserStories.Where(us => us.Title == "Daft story").First();
            UserStory punkStoryUserStory = context.UserStories.Where(us => us.Title == "Punk story").First();

            var Problems = new List<Problem>
            {
                new Problem {Title="My code works and I don't know why",        Description="I just clicked a few times, VS generated plenty of code and it works!... - but I don't know why...",   Importance=Importance.Important,Story=onceUponATimeUserStory, Box=firstProjectBoxes.Where(b=>b.Name=="New").First(),              CreationDate=new DateTime(2017,4,1, 3,2,0), Iteration=firstProjectIteration, EstimatedTime=new TimeSpan(12,0,0)},
                new Problem {Title="My code doesn't work and I don't know why", Description="I tried to do a small refactor but it did broke everything! I cannot undo it and I beg for help.",     Importance=Importance.Critical, Story=onceUponATimeUserStory, Box=firstProjectBoxes.Where(b=>b.Name=="Assigned").First(),         CreationDate=new DateTime(2017,4,1, 4,0,0), Iteration=firstProjectIteration, EstimatedTime=new TimeSpan( 4,0,0)},
                new Problem {Title="Analyse generated classes",                 Description="Ask Kaj for details",                                                                                  Importance=Importance.Regular,  Story=onceUponATimeUserStory, Box=firstProjectBoxes.Where(b=>b.Name=="Waiting for Tests").First(),CreationDate=new DateTime(2017,4,1, 9,0,0), Iteration=firstProjectIteration,  Progress=50,  EstimatedTime=new TimeSpan(12,0,0) },
                new Problem {Title="Learn how to use VS",                       Description="Google for VS tutorial",                                                                               Importance=Importance.Trivial,  Story=onceUponATimeUserStory, Box=firstProjectBoxes.Where(b=>b.Name=="Approved").First(),         CreationDate=new DateTime(2017,4,1,10,0,0), Iteration=firstProjectIteration,  Progress=100, EstimatedTime=new TimeSpan( 4,0,0), FinalTime=new TimeSpan(2,0,0) },

                new Problem {Title="My code works and I don't know why",        Description="I just clicked a few times, VS generated plenty of code and it works!... - but I don't know why...",   Importance=Importance.Important,Story=nextUserStory, Box=newProjectBoxes.Where(b=>b.Name=="New").First(),              CreationDate=new DateTime(2017,4,1, 3,2,0), Iteration=nextProjectIteration, EstimatedTime=new TimeSpan(12,0,0)},
                new Problem {Title="My code doesn't work and I don't know why", Description="I tried to do a small refactor but it did broke everything! I cannot undo it and I beg for help.",     Importance=Importance.Critical, Story=nextUserStory, Box=newProjectBoxes.Where(b=>b.Name=="Assigned").First(),         CreationDate=new DateTime(2017,4,1, 4,0,0), Iteration=nextProjectIteration, EstimatedTime=new TimeSpan( 4,0,0)},
                new Problem {Title="Analyse generated classes",                 Description="Ask Kaj for details",                                                                                  Importance=Importance.Regular,  Story=nextUserStory, Box=newProjectBoxes.Where(b=>b.Name=="Waiting for Tests").First(),CreationDate=new DateTime(2017,4,1, 9,0,0), Iteration=nextProjectIteration,  Progress=50,  EstimatedTime=new TimeSpan(12,0,0) },
                new Problem {Title="Learn how to use VS",                       Description="Google for VS tutorial",                                                                               Importance=Importance.Trivial,  Story=nextUserStory, Box=newProjectBoxes.Where(b=>b.Name=="Approved").First(),         CreationDate=new DateTime(2017,4,1,10,0,0), Iteration=nextProjectIteration,  Progress=100, EstimatedTime=new TimeSpan( 4,0,0), FinalTime=new TimeSpan(2,0,0) },

                new Problem {Title="My code works and I don't know why",        Description="I just clicked a few times, VS generated plenty of code and it works!... - but I don't know why...",   Importance=Importance.Important,Story=daftStoryUserStory, Box=randomAccessMemoriesProjectBoxes.Where(b=>b.Name=="New").First(),              CreationDate=new DateTime(2013,4,1, 3,2,0), Iteration=randomAccessMemoriesIteration, EstimatedTime=new TimeSpan(12,0,0) },
                new Problem { Title = "My code doesn't work and I don't know why", Description = "I tried to do a small refactor but it did broke everything! I cannot undo it and I beg for help.", Importance = Importance.Critical, Story = daftStoryUserStory, Box = randomAccessMemoriesProjectBoxes.Where(b => b.Name == "Assigned").First(), CreationDate = new DateTime(2013, 4, 1, 4, 0, 0), Iteration = randomAccessMemoriesIteration, EstimatedTime = new TimeSpan(4, 0, 0)},
                new Problem {Title="Analyse generated classes",                 Description="Ask Kaj for details",                                                                                  Importance=Importance.Regular,  Story=daftStoryUserStory, Box=randomAccessMemoriesProjectBoxes.Where(b=>b.Name=="Waiting for Tests").First(),CreationDate=new DateTime(2013,4,1, 9,0,0), Iteration=randomAccessMemoriesIteration,  Progress=50,  EstimatedTime=new TimeSpan(12,0,0) },
                new Problem {Title="Learn how to use VS",                       Description="Google for VS tutorial",                                                                               Importance=Importance.Trivial,  Story=daftStoryUserStory, Box=randomAccessMemoriesProjectBoxes.Where(b=>b.Name=="Approved").First(),         CreationDate=new DateTime(2013,4,1,10,0,0), Iteration=randomAccessMemoriesIteration,  Progress=100, EstimatedTime=new TimeSpan( 4,0,0), FinalTime=new TimeSpan(2,0,0) },

                new Problem {Title="My code works and I don't know why",        Description="I just clicked a few times, VS generated plenty of code and it works!... - but I don't know why...",   Importance=Importance.Important,Story=punkStoryUserStory, Box=interstellaProjectBoxes.Where(b=>b.Name=="New").First(),              CreationDate=new DateTime(2019,4,1, 3,2,0), Iteration=interstellaProjectIteration, EstimatedTime=new TimeSpan(12,0,0) },
                new Problem {Title="My code doesn't work and I don't know why", Description="I tried to do a small refactor but it did broke everything! I cannot undo it and I beg for help.",     Importance=Importance.Critical, Story=punkStoryUserStory, Box=interstellaProjectBoxes.Where(b=>b.Name=="Assigned").First(),         CreationDate=new DateTime(2019,4,1, 4,0,0), Iteration=interstellaProjectIteration, EstimatedTime=new TimeSpan( 4,0,0) },
                new Problem {Title="Analyse generated classes",                 Description="Ask Kaj for details",                                                                                  Importance=Importance.Regular,  Story=punkStoryUserStory, Box=interstellaProjectBoxes.Where(b=>b.Name=="Waiting for Tests").First(),CreationDate=new DateTime(2019,4,1, 9,0,0), Iteration=interstellaProjectIteration,  Progress=50,  EstimatedTime=new TimeSpan(12,0,0) },
                new Problem {Title="Learn how to use VS",                       Description="Google for VS tutorial",                                                                               Importance=Importance.Trivial,  Story=punkStoryUserStory, Box=interstellaProjectBoxes.Where(b=>b.Name=="Approved").First(),         CreationDate=new DateTime(2019,4,1,10,0,0), Iteration=interstellaProjectIteration,  Progress=100, EstimatedTime=new TimeSpan( 4,0,0), FinalTime=new TimeSpan(2,0,0) }

            };
            //TODO nie mamy seeda na przypisania User<->Problem
            
            Problems.ForEach(p => context.Problems.Add(p));
            context.SaveChanges();
        }

        private void AddIterations(SeeNowContext context)
        {
            // na razie tylko dwie iteracje z czego jedna będzie pusta (żeby był jakiś wybór)
            //wszystko do backologu na razie
            Project FirstProject = context.Projects.Where(p => p.Name == "First Project").First();
            Project NewProject = context.Projects.Where(p => p.Name == "Next Project").First();
            Project RandomAccessMemoriesProject = context.Projects.Where(p => p.Name == "Random Access Memories Project").First();
            Project InterstellaProject = context.Projects.Where(p => p.Name == "Interstella Project").First();

            List<Iteration> Iterations = new List<Iteration>
            {
                new Iteration {Name="Backlog", Description="Garbage Collector", Project=FirstProject, StartDate=new DateTime(2017,1,1,0,0,0), EndDate=new DateTime(2017, 5, 1, 3, 0, 0)},
                new Iteration {Name="First Iteration", Description="Our team's first itaration. It must be the fastest!", Project=FirstProject, StartDate=new DateTime(2017, 4, 1, 3, 0, 0),  EndDate=new DateTime(2017, 5, 1, 3, 0, 0) },

                new Iteration {Name="Backlog", Description="Garbage Collector", Project=NewProject, StartDate=new DateTime(2017,2,2,0,0,0), EndDate=new DateTime(2017, 8, 1, 3, 0, 0)},
                new Iteration {Name="First Iteration", Description="Our team's first itaration. It must be the fastest!", Project=NewProject, StartDate=new DateTime(2017, 4, 1, 3, 0, 0),  EndDate=new DateTime(2017, 5, 1, 3, 0, 0) },

                new Iteration {Name="Backlog", Description="Garbage Collector", Project=RandomAccessMemoriesProject, StartDate=new DateTime(2013,1,1,0,0,0), EndDate=new DateTime(2013, 5, 1, 3, 0, 0)},
                new Iteration {Name="First Iteration", Description="Our team's first itaration. It must be the fastest!", Project=RandomAccessMemoriesProject, StartDate=new DateTime(2013, 4, 1, 3, 0, 0),  EndDate=new DateTime(2013, 5, 1, 3, 0, 0) },

                new Iteration {Name="Backlog", Description="Garbage Collector", Project=InterstellaProject, StartDate=new DateTime(2019,1,1,0,0,0), EndDate=new DateTime(2019, 5, 1, 3, 0, 0)},
                new Iteration {Name="First Iteration", Description="Our team's first itaration. It must be the fastest!", Project=InterstellaProject, StartDate=new DateTime(2019, 4, 1, 3, 0, 0),  EndDate=new DateTime(2019, 5, 1, 3, 0, 0) }
            };
            Iterations.ForEach(i => context.Iterations.Add(i));
            context.SaveChanges();
        }
        private void AssignUsersToProblems(SeeNowContext context)
        {
            // userow mamy 7, taski mamy 4
            // kazdy user bedzie mial przypisane taski o IDkach bedacych dzielnikami jego ID
            // np. user #6 -> taski 1, 2, 3, user #3 -> taski 1,3 itp
            context.Problems.ToList().ForEach(p =>
            {
                User user;
                for (int i = 1; (user = context.Users.Where(u => u.UserID == i * p.ProblemID).FirstOrDefault()) != null; ++i)
                    p.AssignedUsers.Add(user);
            });
        }
    }
}