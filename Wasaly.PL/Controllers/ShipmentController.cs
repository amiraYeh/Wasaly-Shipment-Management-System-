using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using NuGet.Versioning;
using Org.BouncyCastle.Asn1.Cmp;
using System.Security.Claims;
using System.Threading.Tasks;
using Wasaly.BLL.Services;
using Wasaly.BLL.ViewModels;
using Wasaly.DAL.Data.Context;
using Wasaly.DAL.Models;
using Wasaly.DAL.Repositories;
using Wasaly.DAL.Repositories.IRepositories;

namespace Wasaly.PL.Controllers
{
    [Authorize(Roles = "Merchant")]
    public class ShipmentController : Controller
    {
        private readonly IShipmentService _shipmentService;
        private readonly IConfiguration _configuration;
        private readonly IGoogleMapService _mapService;
        private readonly UserManager<WasalyIdentityUser> _userManager;

        public ShipmentController(IShipmentService shipmentService,IConfiguration configuration,IGoogleMapService mapService, UserManager<WasalyIdentityUser> userManager)
        {
            _shipmentService = shipmentService;
            _configuration = configuration;
            _mapService = mapService;
            _userManager = userManager;
        }
        [Route("Merchant")]
        public async Task<IActionResult> MerchantDashboard()
        {
            string id = _userManager.GetUserId(User);
            if (id == null)
                    return BadRequest();
            var dashboardData = await _shipmentService.GetMerchantDataAsync(id);
            return View(dashboardData);
        }
        public async Task<IActionResult> Index()
        {
            string id = _userManager.GetUserId(User);

            List<ShipmentVM> list = new List<ShipmentVM>();
            list = await _shipmentService.GetShipmentsAsync(id);


            return View(list);
        }
        [HttpGet]
        public IActionResult Add() 
        {
            ViewBag.ApiKey = _configuration["googleMaps:Key"];
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Add(AddShipmentVM shipmentVM)
        {
            if (ModelState.IsValid)
            {
                string id = _userManager.GetUserId(User);

                var res = await _shipmentService.AddAsync(shipmentVM,id);
                if (res != 0)
                {
                    TempData["Success"] = "تم إضافة الشحنة بنجاح 👍👍👍 \nسعر الشحنة : " + res + " جنيه";
                    return RedirectToAction("Index");
                }
            }      
            ViewBag.ApiKey = _configuration["googleMaps:Key"];
            return View(shipmentVM); 
        }
        [HttpGet]
       public async Task<IActionResult> Track(int id)
        {
            if (id == null)
                return BadRequest();
           var res= await _shipmentService.GetDetailAsync(id);
            if(res==null)
                return BadRequest();
            ViewBag.ApiKey = _mapService.GetApi();
            return View(res);
        }
        [HttpGet]
        public async Task<IActionResult> GetDriverLocation(int shipmentId)
        {
            var data = await _shipmentService.GetCurrentLocAsync(shipmentId);
            if (data == null)
                return NotFound();
            return Json(data);
        }
        [HttpPost]
        public async Task<IActionResult> UpdateLocation(int shipmentId, double lat, double lng)
        {
            var shipment = await _shipmentService.GetByIDAsync(shipmentId);
            if (shipment == null)
                return NotFound();
            shipment.CurrentLatitude = lat; 
            shipment.CurrentLongitude = lng;
            int res =await _shipmentService.Update(shipment);
            if(res == 0)
                return NotFound();
            return Ok("Location Updated Successfully");
        }
    }
}
