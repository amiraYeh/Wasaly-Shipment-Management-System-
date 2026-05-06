using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Wasaly.BLL.ViewModels.AdminVM
{
    public class CouriersManagementVM
    {
        public int TotalCount { get; set; }
        public int VerifiedCount { get; set; }
        public int PendingCount { get; set; }
        public int SuspendedCount { get; set; }

        // Table
        public IEnumerable<CourierRowVM> Couriers { get; set; } = new List<CourierRowVM>();

        // Filters
        public string? SearchTerm { get; set; }
        public string? StatusFilter { get; set; }
        public string? RegionFilter { get; set; }
    }
}
