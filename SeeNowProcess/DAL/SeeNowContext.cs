using SeeNowProcess.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;
using System.Linq;
using System.Web;

namespace SeeNowProcess.DAL
{
    public class SeeNowContext : DbContext
    {
        public SeeNowContext() : base("DefaultConnection") //jawna nazwa connection stringa (polen name w webconfigu)
        {

        }
        public DbSet<Problem> Problems { get; set; }
        public DbSet<Project> Projects { get; set; }
        public DbSet<Team> Teams { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<UserStory> UserStories { get; set; }
        public DbSet<Assignment> Assignments { get; set; }
        public DbSet<Box> Boxes { get; set; }



        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();
        }
    }
}