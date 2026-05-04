using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Wasaly.BLL.ViewModels;

namespace Wasaly.BLL
{
 public   interface IAdminService
    {
        Task<AdminStatsVM> GetDashboardStatsAsync();
    }
}
