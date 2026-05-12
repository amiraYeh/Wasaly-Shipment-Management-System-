using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Wasaly.BLL.@interface;
using Wasaly.BLL.Services.Interfaces;

namespace Wasaly.PL.Controllers
{
    //[Area("Admin")]
    [Authorize(Roles = "Admin")]
    public class AdminController : Controller
    {
        private readonly IAdminService _adminService;

  
        public AdminController(IAdminService adminService,IEmailService emailService)

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
        public async Task<IActionResult> UpdateCourierVerification(string id, bool status, string recipientEmail, string recipientName)
        {
            await _adminService.UpdateCourierVerificationAsync(id, status);
            return RedirectToAction("CourierDetails", new { id });
        }

        public async Task<IActionResult> GetMerchants(string? search, string? status)
        {
            var vm = await _adminService.GetMerchantsAsync(search, status);
            return View(vm);
        }

        public async Task<IActionResult> MerchantDetails(string id)
        {
            var vm = await _adminService.GetMerchantDetailsAsync(id);
            if (vm == null) return NotFound();
            return View(vm);
        }

        [HttpPost]
        public async Task<IActionResult> DeleteCourier(string id)
        {
            await _adminService.DeleteCourierAsync(id);
            return RedirectToAction("GetCouriers");
        }

        [HttpPost]
        public async Task<IActionResult> DeleteMerchant(string id)
        {
            await _adminService.DeleteMerchantAsync(id);
            return RedirectToAction("GetMerchants");
        }
    }
}
