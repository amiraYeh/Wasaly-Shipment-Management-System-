using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using Wasaly.BLL.Services;
using Wasaly.BLL.ViewModels;
using Wasaly.DAL.Models;
using Wasaly.DAL.Repositories;
using Wasaly.DAL.Repositories.IRepositories;

namespace Wasaly.PL.Controllers
{
    public class ShipmentController : Controller
    {
        private readonly IShipmentService _shipmentService;
        private readonly IConfiguration _configuration;

        public ShipmentController(IShipmentService shipmentService,IConfiguration configuration)
        {
            _shipmentService = shipmentService;
            _configuration = configuration;
        }
        public async Task<IActionResult> Index()
        {
           
            return View(await _shipmentService.GetShipmentsAsync());
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
                var res = await _shipmentService.AddAsync(shipmentVM);
                if (res != 0)
                {
                    return RedirectToAction("Index","Home");
                }
            }      
            ViewBag.ApiKey = _configuration["googleMaps:Key"];
            return View(); 

        }
    }
}
