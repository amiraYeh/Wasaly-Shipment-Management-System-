using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Wasaly.BLL.ViewModels.AdminVM;

namespace Wasaly.BLL.ViewModels
{
    public class MerchantDashboardVM
    {
        public string MerchantName { get; set; }
        public double? Balance { get; set; }
        public int TodayShipments { get; set; }
        public int MonthShipments { get; set; }
        public double TotalPaid { get; set; }
        public List<ShipmentVM>? RecentShipments { get; set; }
    }
}
