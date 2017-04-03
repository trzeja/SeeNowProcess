using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SeeNowProcess.Models
{
    public class Team
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public virtual User TeamLeader { get; set; }
        public virtual IEnumerable<User> Devs { get; set; }
    }
}