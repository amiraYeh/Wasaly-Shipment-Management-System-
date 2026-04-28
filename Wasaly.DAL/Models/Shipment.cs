using Day9Demo.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Wasaly.DAL.Enums;

namespace Wasaly.DAL.Models
{
  
    public class Shipment
    {
        public int Id { get; set; }

        public string? TrackingNumber { get; set; } = Guid.NewGuid().ToString();
        public string Description { get; set; }

        public double Weight { get; set; }

        public decimal Price { get; set; }

        public ShipmentStatus Status { get; set; }
        // Created | Accepted | PickedUp | Delivered

        public DateTime CreatedAt { get; set; }

        public DateTime? DeliveredAt { get; set; }

        //relationships
        public int MerchantId { get; set; }
        public Merchant Merchant { get; set; }

        // public int? CourierId { get; set; }
        //public Courier Courier { get; set; }

        public int PickupLocationId { get; set; }
        public Location PickupLocation { get; set; }

        public int DropLocationId { get; set; }
        public Location DropLocation { get; set; }

        public int? ShipmentTrackingId { get; set; }
        public ShipmentTracking ShipmentTracking { get; set; }

        public int? CourierAssignmentId { get; set; }
        public CourierAssignment CourierAssignment { get; set; }

        public int? DeliveryOTPId { get; set; }
        public DeliveryOTP DeliveryOTP { get; set; }
    }
}
