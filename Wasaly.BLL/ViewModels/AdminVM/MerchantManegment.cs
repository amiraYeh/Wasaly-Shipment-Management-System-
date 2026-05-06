using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Wasaly.BLL.ViewModels.AdminVM
{
    // ViewModels/AdminVM/MerchantsManagementVM.cs
    public class MerchantsManagementVM
    {
        // Stats
        public int TotalCount { get; set; }
        public int ActiveCount { get; set; }
        public int SuspendedCount { get; set; }

        // Filters
        public string? SearchTerm { get; set; }
        public string? StatusFilter { get; set; }

        // Table
        public IEnumerable<MerchantRowVM> Merchants { get; set; } = new List<MerchantRowVM>();
    }

   
}
