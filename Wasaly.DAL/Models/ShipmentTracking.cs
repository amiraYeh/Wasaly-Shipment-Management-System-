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

        public DateTime TimeStamp { get; set; }

        public string Note { get; set; }

        //relationships
        public List<Shipment> Shipment { get; set; }
    }
}
