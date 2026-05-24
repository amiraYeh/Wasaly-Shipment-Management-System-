using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Wasaly.BLL.Services.Interfaces;
using Wasaly.BLL.ViewModels;
using Wasaly.DAL.Models;

namespace Wasaly.PL.Controllers
{
    [Authorize(Roles = "Courier")]
    public class CourierController : Controller
    {
        private readonly ICourierService _courierService;
        private readonly UserManager<WasalyIdentityUser> _userManager;

        public CourierController(
            ICourierService courierService,
            UserManager<WasalyIdentityUser> userManager)
        {
            _courierService = courierService;
            _userManager   = userManager;
        }

        // ── Dashboard ──────────────────────────────────────────────────
        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var courierId = _userManager.GetUserId(User);
            var dashboard = await _courierService.GetDashboardAsync(courierId);
            ViewData["Balance"] = dashboard.Balance.ToString("F0");
            return View(dashboard);
        }

        // ── Earnings ───────────────────────────────────────────────────
        [HttpGet]
        public async Task<IActionResult> Earnings()
        {
            var courierId = _userManager.GetUserId(User);
            var dashboard = await _courierService.GetDashboardAsync(courierId);
            ViewData["Balance"] = dashboard.Balance.ToString("F0");
            return View(dashboard);
        }

        // ── Available Shipments ────────────────────────────────────────
        [HttpGet]
        public async Task<IActionResult> AvailableShipments()
        {
            var allShipments = await _courierService.GetAvailableShipmentsAsync();
            return View(allShipments);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AcceptShipment(int shipmentId)
        {
            var courierId = _userManager.GetUserId(User);
            var result    = await _courierService.AcceptShipmentAsync(shipmentId, courierId);

            if (result)
                TempData["Success"] = "تم قبول الشحنة بنجاح ✅";
            else
                TempData["Error"] = "الشحنة غير متاحة أو تم قبولها بالفعل";

            return RedirectToAction(nameof(MyShipments));
        }

        // ── My Shipments ───────────────────────────────────────────────
        [HttpGet]
        public async Task<IActionResult> MyShipments()
        {
            var courierId = _userManager.GetUserId(User);
            var shipments = await _courierService.GetCourierShipmentsAsync(courierId);
            return View(shipments);
        }

        // ── Shipment Details ───────────────────────────────────────────
        [HttpGet]
        public async Task<IActionResult> ShipmentDetails(int shipmentId)
        {
            var courierId = _userManager.GetUserId(User);
            var shipment  = await _courierService.GetShipmentDetailsAsync(shipmentId, courierId);

            if (shipment is null)
                return NotFound();

            return View(shipment);
        }

        // ── Pickup ─────────────────────────────────────────────────────
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> PickupShipment(int shipmentId)
        {
            var courierId = _userManager.GetUserId(User);
            var result    = await _courierService.PickupShipmentAsync(shipmentId, courierId);

            if (result)
                TempData["Success"] = "تم تأكيد استلام الشحنة من التاجر ✅";
            else
                TempData["Error"] = "حدث خطأ، تأكد إنك المندوب المخصص لهذه الشحنة";

            return RedirectToAction(nameof(ShipmentDetails), new { shipmentId });
        }

        // ── OTP ────────────────────────────────────────────────────────
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> GenerateOtp(int shipmentId)
        {
            var result = await _courierService.GenerateAndSendOtpAsync(shipmentId);

            if (!result)
            {
                TempData["Error"] = "حدث خطأ أثناء إرسال الكود";
                return RedirectToAction(nameof(ShipmentDetails), new { shipmentId });
            }

            TempData["Success"] = "تم إرسال كود التسليم للعميل ✅";
            return RedirectToAction(nameof(VerifyOtp), new { shipmentId });
        }

        [HttpGet]
        public IActionResult VerifyOtp(int shipmentId)
        {
            var model = new VerifyOtpVM { ShipmentId = shipmentId };
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> VerifyOtp(VerifyOtpVM model)
        {
            if (!ModelState.IsValid)
                return View(model);

            var courierId = _userManager.GetUserId(User);
            var (success, message)= await _courierService.VerifyOtpAndDeliverAsync(model, courierId);

            if (success)
            {
                TempData["Success"] = message;
                
                return RedirectToAction(nameof(ShipmentDetails), new { shipmentId = model.ShipmentId });
            }

            
            TempData["Error"] = message;
            return RedirectToAction(nameof(VerifyOtp), new { shipmentId = model.ShipmentId });
        }
    }
}
