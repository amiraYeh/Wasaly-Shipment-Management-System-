using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Wasaly.DAL.Models;

namespace Wasaly.BLL.ViewModels
{
    public class ShipmentViewModel
    {
        [Required]
        public Location PickupLocation { get; set; }
        [Required]
        public Location DropLocation { get; set; }
        [Required]
        public string Description { get; set; }
        [Required]
        public double Weight { get; set; }
        [Required]
        public DateTime? DeliveredAt { get; set; }
        [Required]
        public decimal Price { get; set; }
    }
}
