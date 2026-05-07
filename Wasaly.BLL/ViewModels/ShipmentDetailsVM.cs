using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using Wasaly.DAL.Enums;
using Wasaly.DAL.Models;

namespace Wasaly.BLL.ViewModels
{
    public class ShipmentDetailsVM
    {
        public int Id { get; set; }
        [Display(Name ="رقم الشحنه")]
        public string TrackingNumber { get; set; }
        
        [Display(Name = "مكان الاستلام")]
        public Location PickLocation { get; set; }

        [Display(Name = "مكان التسليم")]
        public Location DropLocation { get; set; }

        [Display(Name ="الشحنه")]
        public string Description { get; set; }
       
        [Display(Name = "الحالة")]
        public ShipmentStatus Status { get; set; }

        [Display(Name = "المسافة بالكيلومتر")]
        public double? DistanceKm { get; set; }


        [Display(Name = "سجل الشحنه")]
        public List<ShipmentTracking>? History { get; set; }

        //public Location DriverLocation { get; set; }

    }
}
