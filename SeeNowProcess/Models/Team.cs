using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SeeNowProcess.Models
{
    public class Team
    {
        public int ID { get; set; }
        public int TeamLeaderID { get; set; }
        public int UserStoryID { get; set; }
        public string Name { get; set; }
        public virtual User TeamLeader { get; set; }
        public virtual UserStory UserStory { get; set; }
        public virtual ICollection<User> Devs { get; set; }
        
    }
}