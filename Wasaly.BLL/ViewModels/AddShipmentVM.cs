using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Wasaly.BLL.Attributes;
using Wasaly.DAL.Models;

namespace Wasaly.BLL.ViewModels
{
    public class AddShipmentVM
    {
        //public string? TrackingNumber { get; set; } 
        [Required]
        [Display(Name ="مكان الاستلام")]
        public Location PickupLocation { get; set; }
        [Required]
        [Display(Name ="مكان التسليم")]
        public Location DropLocation { get; set; }
        [Required]
        [Display(Name ="وصف الشحنه")]
        public string Description { get; set; }
        [Required]
        [Display(Name ="الوزن")]
        [Range(1,100)]
        public double Weight { get; set; }
        public DateTime myDate { get; set; } = DateTime.Now;
        [Required]
        [Display(Name = "معاد التسليم")]
        [FutureDate( ErrorMessage ="تاريخ التسليم يجب أن يكون في المستقبل")]
        public DateTime? DeliveredAt { get; set; } = DateTime.Now;
        [Required]
        [Display(Name ="اسم المشترى")]
        [StringLength(50,MinimumLength =5)]
        public string RecipientName { get; set; }
        [Required]
        [Display(Name = "ايميل المشترى")]
        [DataType(DataType.EmailAddress)]
        public string RecipientEmail { get; set; }
    }
}
