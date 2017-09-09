using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using SeeNowProcess.Models.Enums;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SeeNowProcess.Models
{
    public class User
    {
        public User()
        {
            Assignments = new List<Assignment>();
            Stories = new List<UserStory>();
            Problems = new List<Problem>();
        }
        
        public int UserID { get; set; }
        
        [StringLength(20, MinimumLength=5),Required]
        public string Login { get; set; }
        [StringLength(100, MinimumLength=6),Required] // 6, to accept password "admin1" :)
        //[DataType(DataType.Password)] // w sumie nie wiem co to dokladnie robi, ale typ pasuje
        [NotMapped]
        public string Password {
            set {
                this.SetPassword(value);
            }
            get {
                return Hash==null ? null : Hash.ToString(); // cos musi byc; generalnie nie uzywac tego gettera
            }
        }
        public byte[] Salt { get; set; }
        public byte[] Hash { get; set; }
        [StringLength(50, MinimumLength = 4), Required]
        public string Name { get; set; }
        [Display(Name="E-mail address")]
        [DataType(DataType.EmailAddress),Required]       
        public string Email { get; set; }
        [Display(Name="Phone Number")]
        [DataType(DataType.PhoneNumber)]
        public string PhoneNumber { get; set; } //zazwyczaj nie wymaga sie
        [Required]
        public Role role { get; set; }
        public virtual ICollection<Assignment> Assignments { get; set; }
        public virtual ICollection<UserStory> Stories { get; set; }
        public virtual ICollection<Problem> Problems { get; set; }

        public Boolean ComparePassword(String attemptedPassword)
        {
            if (Hash == null || Salt == null)
                return false;
            return CryptoServices.CheckPassword(attemptedPassword, Hash, Salt);
        }

        public Boolean SetPassword(String newPassword)
        {
            byte[] newSalt;
            byte[] newHash;
            try {
                newSalt = CryptoServices.GenerateSalt();
                newHash = CryptoServices.Encrypt(newPassword, newSalt);
            } catch (Exception)
            {
                return false;
            }
            Hash = newHash;
            Salt = newSalt;
            return true;
        }
    }
}