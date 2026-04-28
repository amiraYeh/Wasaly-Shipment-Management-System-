using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Wasaly.DAL.Models
{
    public class DeliveryOTP
    {
        public int Id { get; set; }

        public int OTPCode { get; set; }

        public DateTime ExpiryTime { get; set; }

        public bool IsUsed { get; set; } = false;

        public DateTime CreatedAt { get; set; }

        //relationships
        public int ShipmentId { get; set; }
        public Shipment Shipment { get; set; }
    }
}
