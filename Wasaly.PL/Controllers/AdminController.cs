using Microsoft.AspNetCore.Mvc;
using Wasaly.BLL.@interface;

namespace Wasaly.PL.Controllers
{
    //[Area("Admin")]
    public class AdminController : Controller
    {
        private readonly IAdminService _adminService;

  
        public AdminController(IAdminService adminService)

        {

            _adminService = adminService;

        }


        public async Task<IActionResult> Index()

        {

            var data = await _adminService.GetDashboardStatsAsync();


            return View(data);

        }
    }
}
