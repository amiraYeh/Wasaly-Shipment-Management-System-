using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Wasaly.BLL.ViewModels
{
    public class CourierDashboardVM
    {
        public string CourierName { get; set; }
        public decimal Balance { get; set; }
        public int TodayDeliveries { get; set; }
        public decimal WeekEarnings { get; set; }
        public double Rating { get; set; }
        public int AcceptanceRate { get; set; }
        public List<CourierShipmentVM> RecentShipments { get; set; } = new();
    }
}
