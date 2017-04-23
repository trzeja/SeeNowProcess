using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity;
using SeeNowProcess.Models;
using SeeNowProcess.DAL;
using SeeNowProcess.Tests.DAL.DbSets;

namespace SeeNowProcess.Tests.DAL
{
    public class TestSeeNowContext : ISeeNowContext
    {
        public TestSeeNowContext()
        {
            Users = new TestUserDbSet();
        }

        public DbSet<Problem> Problems { get; set; }
        public DbSet<Project> Projects { get; set; }
        public DbSet<Team> Teams { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<UserStory> UserStories { get; set; }
        public DbSet<Assignment> Assignments { get; set; }
        public DbSet<Box> Boxes { get; set; }

        public int SaveChanges()
        {
            return 0;
        }

        public void Dispose() { }


    }
}
