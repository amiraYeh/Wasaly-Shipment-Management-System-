using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Wasaly.BLL.ViewModels.AdminVM
{
    public class MerchantRowVM
    {
        public string Id { get; set; } = null!;
        public string FullName { get; set; } = null!;
        public string StoreName { get; set; } = null!;
        public string BusinessType { get; set; } = null!;
        public string PhoneNumber { get; set; } = null!;
        public string Region { get; set; } = null!;
        public string Email { get; set; } = null!;
        public int ShipmentsCount { get; set; }
        public bool IsSuspended { get; set; }
        public IEnumerable<ShipmentRowVM> RecentShipments { get; set; } = new List<ShipmentRowVM>(); 

        // Helpers
        public string StatusText => IsSuspended ? "موقوف" : "نشط";
        public string StatusClass => IsSuspended ? "bg-danger" : "bg-success";
    }
   
}
