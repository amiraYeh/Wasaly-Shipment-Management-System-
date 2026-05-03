using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Wasaly.DAL.Models;

namespace Wasaly.BLL.ViewModels
{
    public class ShipmentVM
    {
        [Display(Name ="رقم الشحنة")]
        public string TrackingNumber { get; set; }

        [Display(Name ="العنوان")]
        public string DropLocation { get; set; }

        [Display(Name = "المندوب")]
        public string CourierAssignmentName { get; set; }

        [Display(Name = "التقيم")]
        public int CourierAssignmentRate { get; set; }

    }
}
