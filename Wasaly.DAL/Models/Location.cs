using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Wasaly.DAL.Models
{
    public class Location
    {
        public int Id { get; set; }

        public string Address { get; set; }

        public double Latitude { get; set; }

        public double Longitude { get; set; }

        //relationships
        public ICollection<Shipment> PickupShipments { get; set; }
        public ICollection<Shipment> DropShipments { get; set; }

    }
}
