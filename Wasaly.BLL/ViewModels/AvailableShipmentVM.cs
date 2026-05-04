using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Wasaly.BLL.ViewModels
{
    public class AvailableShipmentVM
    {
        public int Id { get; set; }
        public string TrackingNumber { get; set; }
        public string Description { get; set; }
        public double Weight { get; set; }
        public decimal Price { get; set; }

        public string MerchantName { get; set; }

        public string PickupAddress { get; set; }
        public double PickupLatitude { get; set; }
        public double PickupLongitude { get; set; }

        public string DropAddress { get; set; }
        public double DropLatitude { get; set; }
        public double DropLongitude { get; set; }

        public DateTime CreatedAt { get; set; }
    }
}
