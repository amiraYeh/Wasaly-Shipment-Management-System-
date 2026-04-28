using Day9Demo.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Wasaly.DAL.Enums;

namespace Wasaly.DAL.Models
{  
    public class CourierAssignment
    {
        public int Id { get; set; }
        public int CourierId { get; set; }
        public Courier Courier { get; set; }

        public DateTime AssignedAt { get; set; }

        public CourierStatus Status { get; set; }

        //relationships
        public int ShipmentId { get; set; }
        public Shipment Shipment { get; set; }
    }
}
