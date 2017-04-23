﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace SeeNowProcess.Models
{
    public class Team
    {
        
        public int TeamID { get; set; }
        
        public string Name { get; set; }
        public virtual User TeamLeader { get; set; }
        public virtual UserStory UserStory { get; set; }
        public virtual IQueryable<Assignment> Assignments { get; set; }

    }
}