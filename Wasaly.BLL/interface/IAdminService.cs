using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Wasaly.BLL.ViewModels;
using Wasaly.BLL.ViewModels.AdminVM;

namespace Wasaly.BLL.@interface
{
 public   interface IAdminService
    {
        Task<AdminStatsVM> GetDashboardStatsAsync();
    }
}
