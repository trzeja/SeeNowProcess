using SeeNowProcess.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Data.Entity.ModelConfiguration.Conventions;
using System.Data.Entity.Validation;
using System.Linq;
using System.Reflection;
using System.Web;

namespace SeeNowProcess.DAL
{
    public class SeeNowContext : DbContext, ISeeNowContext
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
        public DbSet<Iteration> Iterations { get; set; }

        public void MarkAsModified<TEntity>(TEntity entity) where TEntity : class
        {
            Entry(entity).State = EntityState.Modified;
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();
        }
        public override int SaveChanges()
        {
            var errors = this.GetValidationErrors();
            foreach (DbEntityValidationResult result in errors)
            {
                Type baseType = result.Entry.Entity.GetType().BaseType;
                //skrocona wersja zakomentowanego - laduje tylko property z bledami
                foreach(DbValidationError valErr in result.ValidationErrors)
                {
                    baseType.GetProperty(valErr.PropertyName).GetValue(result.Entry.Entity, null);
                }
                //foreach (PropertyInfo property in result.Entry.Entity.GetType().GetProperties())
                //{
                //    if (baseType.GetProperty(property.Name).GetCustomAttributes(typeof(RequiredAttribute), true).Any())
                //    {
                //        property.GetValue(result.Entry.Entity, null);
                //    }
                //}
            }
            return base.SaveChanges();
        }
    }
}