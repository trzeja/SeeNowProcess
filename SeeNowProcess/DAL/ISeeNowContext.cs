using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity;
using SeeNowProcess.Models;

namespace SeeNowProcess.DAL
{
    public interface ISeeNowContext : IDisposable
    {
        DbSet<Problem> Problems { get; set; }
        DbSet<Project> Projects { get; set; }
        DbSet<Team> Teams { get; set; }
        DbSet<User> Users { get; set; }
        DbSet<UserStory> UserStories { get; set; }
        DbSet<Assignment> Assignments { get; set; }
        DbSet<Box> Boxes { get; set; }

        int SaveChanges();
        //void OnModelCreating(DbModelBuilder modelBuilder);
    }
}
