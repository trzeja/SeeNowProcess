using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using SeeNowProcess.Models.Enums;
using System.ComponentModel.DataAnnotations;

namespace SeeNowProcess.Models
{
    public class User
    {
        public int Id { get; set; }
        [StringLength(20, MinimumLength=5)]
        public string Login { get; set; }
        [StringLength(100, MinimumLength=6)] // 6, to accept password "admin1" :)
        [DataType(DataType.Password)] // w sumie nie wiem co to dokladnie robi, ale typ pasuje
        public string Password { get; set; }
        public string Name { get; set; }
        [Display(Name="E-mail address")]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }
        [Display(Name="Phone Number")]
        [DataType(DataType.PhoneNumber)]
        public string PhoneNumber { get; set; }
        public Role role { get; set; }
        public User supervisor { get; set; }
        public virtual IEnumerable<Team> Teams { get; set; }
        public virtual IEnumerable<User> Subordinates { get; set; } // users whose supervisor I am
    }
}