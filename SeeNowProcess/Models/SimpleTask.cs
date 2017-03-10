using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace SeeNowProcess.Models
{
    public class SimpleTask
    {
        public int ID { get; set; }
        public Importance Importance { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public State CurrentState { get; set; }
        public DateTime CreationDate { get; set; }
        public DateTime CompletionDate { get; set; }        
        public decimal Progress { get; set; }
    }

    public enum State
    {
        Created,
        InProgress,
        Finished
    }

    public enum Importance
    {
        None,
        Trivial,
        Regular,
        Important,
        Critical
    };

    public class SimpleTaskDBContext : DbContext
    {
        public DbSet<SimpleTask> SimpleTasks { get; set; }
    }
}