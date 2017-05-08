using SeeNowProcess.Models.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace SeeNowProcess.Models
{
    public class Problem
    {
        public Problem()
        {
            AssignedUsers = new List<User>();
        }

        public int ProblemID { get; set; }
        

        [StringLength(60, MinimumLength = 3)]
        public string Title { get; set; }
        public string Description { get; set; }

        //[Display(Name = "Current State")]
        //public State? CurrentState { get; set; }
        public Importance? Importance { get; set; }
        public int? Progress { get; set; }

        [Display(Name = "Creation Date")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime? CreationDate { get; set; }
        public virtual Box Box { get; set; }

        [Display(Name = "Parent Problem")]
        public virtual Problem ParentProblem { get; set; }

        [Display(Name = "Estimated Time")]
        public TimeSpan? EstimatedTime { get; set; } //new TimeSpan(2, 14, 18); "02:14:18"

        [Display(Name = "Final Time")]
        public TimeSpan? FinalTime { get; set; }
        public virtual UserStory Story { get; set; }
        public virtual ICollection<User> AssignedUsers { get; set; }
        
    } 
}