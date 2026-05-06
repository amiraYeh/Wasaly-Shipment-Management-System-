using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Wasaly.BLL.ViewModels;
using Wasaly.BLL.ViewModels.AdminVM;

namespace Wasaly.BLL
{
 public   interface IAdminService
    {
        Task<AdminStatsVM> GetDashboardStatsAsync();
        Task<CouriersManagementVM> GetCouriersAsync(string? search, string? status, string? region);
        Task<CourierRowVM?> GetCourierDetailsAsync(string id);
        Task<bool> UpdateCourierVerificationAsync(string id, bool status);

    }
}
