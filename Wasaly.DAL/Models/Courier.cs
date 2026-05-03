using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Wasaly.DAL.Models;

namespace Day9Demo.Models
{
    public class Courier
    {
        [Key]
        [ForeignKey("WasalyIdentityUser")]
        public string WasalyIdentityUserId { get; set; } = null!;
        public virtual WasalyIdentityUser WasalyIdentityUser { get; set; } = null!;

        public string NationalIdImagePath { get; set; } = null!;
        public string DrivingLicenseImagePath { get; set; } = null!;
        public string ProfileImagePath { get; set; } = null!;
        public bool isVerfied { get; set; }
        public ICollection<CourierAssignment> assignments { get; set; }
        public decimal Balance { get; set; }    
    }

}
