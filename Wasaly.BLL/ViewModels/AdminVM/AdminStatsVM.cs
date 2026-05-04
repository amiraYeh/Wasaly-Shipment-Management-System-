using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Wasaly.BLL.ViewModels
{
    public class AdminStatsVM
    {
        public decimal MonthlyRevenue { get; set; }
        public int TodayShipmentsCount { get; set; }
        public int TotalMerchantsCount { get; set; }
        public int TotalCouriersCount { get; set; }
        public int OpenDisputesCount { get; set; }
    }
}
