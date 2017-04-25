using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity;
using SeeNowProcess.Models;
using System.Data.Entity.Infrastructure;

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

        /// <summary>
        /// Marks entity as modified - useful in edit actions
        /// Example: db.MarkAsModified<User>(allUsers.FirstOrDefault());
        /// </summary>
        /// <typeparam name="TEntity">
        /// Write here class from  model eg. User
        /// </typeparam>
        /// <param name="entity"></param>
        void MarkAsModified<TEntity>(TEntity entity) where TEntity : class;
        //void OnModelCreating(DbModelBuilder modelBuilder);
    }
}
