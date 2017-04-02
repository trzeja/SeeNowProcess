﻿using SeeNowProcess.Models.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace SeeNowProcess.Models
{
    public class Project
    {
        public int Id { get; set; }

        [StringLength(60, MinimumLength = 3)]
        public string Name { get; set; }
        public string Description { get; set; }

        [Display(Name = "Start Date")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime StartDate { get; set; }


        [Display(Name = "Completion Date")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime CompletionDate { get; set; }
        public Status Status { get; set; }

        public IEnumerable<UserStory> Stories { get; set; }
    }
}