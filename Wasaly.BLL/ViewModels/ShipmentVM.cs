using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Wasaly.DAL.Enums;
using Wasaly.DAL.Models;

namespace Wasaly.BLL.ViewModels
{
    public class ShipmentVM
    {
        public int Id { get; set; }
        [Display(Name ="رقم الشحنة")]
        public string TrackingNumber { get; set; }

        [Display(Name ="العنوان")]
        public string DropLocation { get; set; }

        [Display(Name = "المندوب")]
        public string CourierAssignmentName { get; set; }

        [Display(Name = "الحالة")]
        public ShipmentStatus Status { get; set; }

        [Display(Name = "التقيم")]
        public int CourierAssignmentRate { get; set; }

        [Display(Name = "السعر")]
        public decimal Price { get; set; }
    }
}
