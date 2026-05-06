using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Wasaly.BLL.ViewModels.AdminVM
{
    public class ShipmentRowVM
    {
        public string Id { get; set; } = null!;
        public string CourierName { get; set; } = null!;
        public string Status { get; set; } = null!;
        public string CreatedAt { get; set; } = null!;
    }
}
