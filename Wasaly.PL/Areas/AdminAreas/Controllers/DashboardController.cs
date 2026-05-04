using Microsoft.AspNetCore.Mvc;
using Wasaly.BLL;

namespace Wasaly.PL.Areas.AdminAreas.Controllers
{
    [Area("AdminAreas")]
    public class DashboardController : Controller
    {
        private readonly IAdminService _adminService;

  
        public DashboardController(IAdminService adminService)

        {

            _adminService = adminService;

        }


        public async Task<IActionResult> AdminDashBord()

        {

            var data = await _adminService.GetDashboardStatsAsync();


            return View(data);

        }
    }
}
