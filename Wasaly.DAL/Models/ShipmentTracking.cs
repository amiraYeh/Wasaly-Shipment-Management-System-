using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Wasaly.DAL.Enums;

namespace Wasaly.DAL.Models
{
    public class ShipmentTracking
    {
        public int Id { get; set; }

        public ShipmentStatus Status { get; set; } 

        public DateTime TimeStamp { get; set; } = DateTime.Now;

        public string Note { get; set; }

        //relationships
        public int? ShipmentId { get; set; }
        public Shipment? Shipment { get; set; }
    }
}
