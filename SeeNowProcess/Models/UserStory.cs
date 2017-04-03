﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace SeeNowProcess.Models
{
    public class UserStory
    {
        public int ID { get; set; }
        public int OwnerID { get; set; }
        public int ProjectID { get; set; }

        [StringLength(60, MinimumLength = 3)]
        public string Title { get; set; }
        public string Description { get; set; }
        public int Size { get; set; }
        public string Unit { get; set; }
        public string Notes { get; set; }
        public User Owner {get;set;}
        public virtual ICollection<Team> Teams { get; set; } //to
        //public Team Team { get; set; }        //lub to
        public string Criteria { get; set; }
        public virtual Project Project { get; set; }

    }
}