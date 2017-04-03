using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SeeNowProcess.Models
{
    public class Assignment
    {
        public int AssignmentID { get; set; }
        public int UserID { get; set; }
        public int TeamID { get; set; }

        public virtual User User { get; set; }
        public virtual Team Team { get; set; }

    }
}