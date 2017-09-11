﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
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
            UserStories = new List<UserStory>();
        }
        
        public int TeamID { get; set; }
        [StringLength(60, MinimumLength = 3)]
        [Required]
        public string Name { get; set; }
        public virtual User TeamLeader { get; set; }
        public virtual ICollection<UserStory> UserStories { get; set; }
        public virtual ICollection<Assignment> Assignments { get; set; }

    }
}