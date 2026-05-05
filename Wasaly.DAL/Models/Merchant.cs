using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Wasaly.DAL.Models;

namespace Wasaly.DAL.Models
{
    public class Merchant
    {
        [Key]
        [ForeignKey("WasalyIdentityUser")]
        public string WasalyIdentityUserId { get; set; } = null!;

        public virtual WasalyIdentityUser WasalyIdentityUser { get; set; } = null!;

        public string StoreName { get; set; } = null!;

        public ICollection<Shipment> shipments { get; set; }

        public string BusinessType { get; set; } = null!;
    }
}
