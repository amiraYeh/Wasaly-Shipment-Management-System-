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
        [Required(ErrorMessage = "Store name is required")]
        [StringLength(100, MinimumLength = 3, ErrorMessage = "Store name must be between 3 and 100 characters")]
        public string StoreName { get; set; } = null!;

        [Required(ErrorMessage = "Store address is required")]
        [StringLength(200, MinimumLength = 5, ErrorMessage = "Address must be between 5 and 200 characters")]
        public string StoreAddress { get; set; } = null!;

        [Required(ErrorMessage = "Business type is required")]
        [StringLength(50, ErrorMessage = "Business type can't exceed 50 characters")]
        public string BusinessType { get; set; } = null!;

    }
}
