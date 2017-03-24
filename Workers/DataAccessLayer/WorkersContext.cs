using System.Data.Entity;
using Workers.Models;
using System.Data.Entity.ModelConfiguration.Conventions;

namespace Workers.DataAccessLayer
{
    public class WorkersContext : DbContext
    {
        public WorkersContext() : base("WorkersContext")
        {
        }

        public DbSet<Worker> Workers { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();

            modelBuilder.Entity<Worker>()
                        .HasOptional(i => i.Supervisor)
                        .WithMany(i => i.Subordinates)
                        .HasForeignKey(i => i.SupervisorId);
        }
    }
}