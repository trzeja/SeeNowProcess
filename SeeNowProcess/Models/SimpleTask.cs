using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
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

        [Display(Name = "Current State")]        
        public State CurrentState { get; set; }

        [Display(Name = "Creation Date")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime CreationDate { get; set; }

        [Display(Name = "Completion Date")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
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