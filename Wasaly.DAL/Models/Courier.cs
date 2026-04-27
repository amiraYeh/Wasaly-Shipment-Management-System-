using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Wasaly.DAL.Models;

namespace Day9Demo.Models
{
    public class Merchant
    {
        [Key]
        [ForeignKey("WasalyIdentityUser")]
        public string WasalyIdentityUserId { get; set; } = null!;
        public virtual WasalyIdentityUser WasalyIdentityUser { get; set; } = null!;

    }
}
