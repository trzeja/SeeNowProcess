using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SeeNowProcess.Models
{
    public class Box
    {
        //TODO (mozliwe ze doimplementowac cos)
        public int BoxID { get; set; }
        public int Order { get; set; }
        public string Name { get; set; }
        public virtual  Project Project { get; set; }
        public virtual IQueryable<Problem> Problems { get; set; }
    }
}