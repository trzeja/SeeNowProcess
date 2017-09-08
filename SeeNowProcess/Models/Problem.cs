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
        

        [StringLength(60, MinimumLength = 3),Required]
        public string Title { get; set; }
        [StringLength(200, MinimumLength = 3), Required]
        public string Description { get; set; }
        [Required]
        public Importance Importance { get; set; }
        [Required]
        public int Progress { get; set; }

        [Display(Name = "Creation Date")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime CreationDate { get; set; }
        [Required]
        public virtual Box Box { get; set; }
        [Required]
        public virtual Iteration Iteration { get; set; }
        
        [Display(Name = "Estimated Time")]
        [Required]
        public TimeSpan EstimatedTime { get; set; } //new TimeSpan(2, 14, 18); "02:14:18"

        [Display(Name = "Final Time")]
        public TimeSpan? FinalTime { get; set; }
        [Required]
        public virtual UserStory Story { get; set; }
        [Required]
        public virtual ICollection<User> AssignedUsers { get; set; }
        
    } 
}