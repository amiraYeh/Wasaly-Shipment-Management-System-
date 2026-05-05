using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Wasaly.BLL.Services;
using Wasaly.DAL.Models;

namespace Wasaly.PL.ViewComponents
{
    public class LocationViewComponent:ViewComponent
    {
        private readonly IShipmentService _shipmentService;

        public LocationViewComponent(IShipmentService shipmentService)
        {
            _shipmentService = shipmentService;
        }
        public async Task<IViewComponentResult> Invoke(int id)
        {
            var shipment = await _shipmentService.GetDetailAsync(id);
            if(shipment == null)
                return View("Error");
            return View("~/Views/Shipment/_AddressSearch.cshtml",shipment.Location);
        }
    }
}
