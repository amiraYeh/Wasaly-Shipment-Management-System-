using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Wasaly.DAL.Models;

namespace Wasaly.BLL.ViewModels
{
    public class ShipmentDetailsVM
    {
        public int Id { get; set; }
        public string TrackingNumber { get; set; }
        public Location Location { get; set; }
    }
}
