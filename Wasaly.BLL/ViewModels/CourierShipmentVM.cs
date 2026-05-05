using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Wasaly.DAL.Enums;

namespace Wasaly.BLL.ViewModels
{
    public class CourierShipmentVM
    {
        public int ShipmentId { get; set; }
        public int AssignmentId { get; set; }
        public string TrackingNumber { get; set; }
        public string Description { get; set; }
        public double Weight { get; set; }
        public decimal Price { get; set; }

        public ShipmentStatus ShipmentStatus { get; set; }
        public CourierStatus AssignmentStatus { get; set; }

        public string PickupAddress { get; set; }
        public string DropAddress { get; set; }

        public DateTime AssignedAt { get; set; }

        public List<TrackingHistoryVM> TrackingHistory { get; set; } = new();
    }
    public class TrackingHistoryVM
    {
        public ShipmentStatus Status { get; set; }
        public string StatusArabic { get; set; }
        public DateTime TimeStamp { get; set; }
        public string Note { get; set; }
    }
}
