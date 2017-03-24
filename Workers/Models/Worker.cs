using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Workers.Models
{
    public class Worker
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        [ForeignKey("Supervisor")]
//        [ForeignKey("Worker")]
        public Nullable<int> SupervisorId { get; set; }
        public string FullName
        {
            get
            {
                return FirstName + " " + LastName;
            }
        }
        public string LastName { get; set; }
        public string FirstName { get; set; }

        public virtual ICollection<Worker> Subordinates { get; set; }
        public virtual Worker Supervisor {get; set;}

    }
}