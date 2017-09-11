using SeeNowProcess.Models.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace SeeNowProcess.Models
{
    public class Project
    {
        public Project()
        {
            Stories = new List<UserStory>();
            Iterations = new List<Iteration>();
            Boxes = new List<Box>();
        }

        public int ProjectID { get; set; }

        [StringLength(60, MinimumLength = 3)]
        [Required]
        public string Name { get; set; }
        [StringLength(200, MinimumLength = 3), Required]
        public string Description { get; set; }

        [Display(Name = "Start Date")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        [Required]
        public DateTime StartDate { get; set; }


        [Display(Name = "Completion Date")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime? CompletionDate { get; set; }
        [Required]
        public Status Status { get; set; }

        public virtual ICollection<UserStory> Stories { get; set; }
        public virtual ICollection<Iteration> Iterations { get; set; }
        public virtual ICollection<Box> Boxes { get; set; }
    }
}