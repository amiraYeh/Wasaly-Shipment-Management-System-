using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Wasaly.DAL.Models;

namespace Day9Demo.Models
{
    public class Merchant
    {
        public string WasalyIdentityUserId { get; set; } = null!;

        public virtual WasalyIdentityUser WasalyIdentityUser { get; set; } = null!;

        public string StoreName { get; set; } = null!;

        public string StoreAddress { get; set; } = null!;

        public string BusinessType { get; set; } = null!;
    }
}
