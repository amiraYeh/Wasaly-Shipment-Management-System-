using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Wasaly.DAL.Models;

namespace Wasaly.BLL.ViewModels
{
    public class AddShipmentVM
    {
        //public string? TrackingNumber { get; set; } 
        //[Required]
        [Display(Name ="مكان الاستلام")]
        public Location PickupLocation { get; set; }
        //[Required]
        [Display(Name ="مكان التسليم")]
        public Location DropLocation { get; set; }
        [Required]
        [Display(Name ="وصف الشحنه")]
        public string Description { get; set; }
        [Required]
        [Display(Name ="الوزن")]
        public double Weight { get; set; }
        [Required]
        [Display(Name ="معاد التسليم")]
        public DateTime? DeliveredAt { get; set; }
        //[Required]
        //[Display(Name = "السعر")]
        //public decimal Price { get; set; }
    }
}
