using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace SeeNowProcess.Models
{
    public class UserStory
    {
        public UserStory()
        {
            Teams = new List<Team>();
        }
        
        public int UserStoryID { get; set; }
      

        [StringLength(60, MinimumLength = 3)]
        [Required]
        public string Title { get; set; }
        [StringLength(200, MinimumLength = 3)]
        [Required]
        public string Description { get; set; }
        [Required, Range(1,int.MaxValue)]
        public int Size { get; set; }
        [Required]
        public string Unit { get; set; }
        [StringLength(200, MinimumLength = 3)]
        public string Notes { get; set; }
        //[Required]
        public virtual User Owner {get;set;}
        [Required]
        public virtual ICollection<Team> Teams { get; set; } 
        public virtual ICollection<Problem> Problems { get; set; }       
        [StringLength(200, MinimumLength = 3), Required]
        public string Criteria { get; set; }
        //[Required]
        public virtual Project Project { get; set; }

    }
}