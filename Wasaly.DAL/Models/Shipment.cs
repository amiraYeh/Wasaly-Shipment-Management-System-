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

        public string? TrackingNumber { get; set; }= Guid.NewGuid().ToString("N").Substring(0, 7);

        public string Description { get; set; }

        public double Weight { get; set; }

        public decimal Price { get; set; }

        public ShipmentStatus Status { get; set; }= ShipmentStatus.Created;

        public DateTime CreatedAt { get; set; } = DateTime.Now;

        public DateTime? DeliveredAt { get; set; }

        // معلومات العميل
        public string RecipientName { get; set; }
        public string RecipientEmail { get; set; }
        public string RecipientPhone { get; set; }

        // Relationships


        public string? MerchantId { get; set; }
        public Merchant? Merchant { get; set; }

        public int? PickupLocationId { get; set; }
        public Location? PickupLocation { get; set; }

        public int? DropLocationId { get; set; }
        public Location? DropLocation { get; set; }

        public ICollection<ShipmentTracking>? Trackings { get; set; }

        public ICollection<CourierAssignment>? CourierAssignments { get; set; }

        public DeliveryOTP? DeliveryOTP { get; set; }
    }
}