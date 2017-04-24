using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity;
using SeeNowProcess.Models;
using SeeNowProcess.DAL;
using SeeNowProcess.Tests.DAL.DbSets;
using SeeNowProcess.Tests.DAL.DbSets.Base;

namespace SeeNowProcess.Tests.DAL
{
    public class TestSeeNowContext : ISeeNowContext
    {
        public TestSeeNowContext()
        {
            Users = new TestDbSet<User>();
            //Users = new TestUserDbSet();
            int asd;
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
            return 0; // no real db, no sql
        }
        
        public void Dispose() { }


    }
}
