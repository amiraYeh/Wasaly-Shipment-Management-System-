using Microsoft.AspNetCore.Mvc;
using Wasaly.BLL;

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
        public async Task<IActionResult> GetCouriers(string? search, string? status, string? region)

        {
            var vm = await _adminService.GetCouriersAsync(search, status, region);
            return View(vm);
        }

        // AdminController.cs
        public async Task<IActionResult> CourierDetails(string id)
        {
            var vm = await _adminService.GetCourierDetailsAsync(id);
            if (vm == null) return NotFound();
            return View(vm);
        }

        [HttpPost]
        public async Task<IActionResult> UpdateCourierVerification(string id, bool status)
        {
            await _adminService.UpdateCourierVerificationAsync(id, status);
            return RedirectToAction("CourierDetails", new { id });
        }
    }
}
