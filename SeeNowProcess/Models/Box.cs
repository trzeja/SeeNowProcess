using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SeeNowProcess.Models
{
    public class Box
    {
        public Box()
        {
            Problems = new List<Problem>();
        }        

        //TODO (mozliwe ze doimplementowac cos)
        public int BoxID { get; set; }
        public int Order { get; set; }
        public string Name { get; set; }
        public virtual  Iteration Iteration { get; set; }
        public virtual ICollection<Problem> Problems { get; set; }
    }
}