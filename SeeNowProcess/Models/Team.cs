using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace SeeNowProcess.Models
{
    public class Team
    {
        public Team()
        {
            Assignments = new List<Assignment>();
        }
        
        public int TeamID { get; set; }
        
        public string Name { get; set; }
        public virtual User TeamLeader { get; set; }
        public virtual ICollection<UserStory> UserStories { get; set; }
        public virtual ICollection<Assignment> Assignments { get; set; }

    }
}