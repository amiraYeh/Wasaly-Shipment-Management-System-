using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using System.Text;
using System.Threading.Tasks;
using Wasaly.BLL.ViewModels;
using Wasaly.DAL.Models;
using Wasaly.DAL.Repositories.IRepositories;

namespace Wasaly.BLL.Services
{
    public class ShipmentService:IShipmentService
    {
        private readonly IShipmentRepository _shipmentRepository;
        private readonly IGoogleMapService _mapService;

        public ShipmentService(IShipmentRepository shipmentRepository,IGoogleMapService mapService)
        {
            _shipmentRepository = shipmentRepository;
            _mapService = mapService;
        }
        public async Task<List<ShipmentVM>> GetShipmentsAsync()
        {
            var shipments = await _shipmentRepository.GetAllAsync();
            List<ShipmentVM> shipmentList = new List<ShipmentVM>();
            foreach (var ship in shipments)
            {
                var courdata = await _shipmentRepository.GetCourierData(ship.Id);
                if (courdata != null)
                {
                    shipmentList.Add(new ShipmentVM
                    {
                        TrackingNumber = ship.TrackingNumber,
                        CourierAssignmentName = courdata.Item1,
                        CourierAssignmentRate = (int)courdata.Item2,
                        DropLocation = ship.DropLocation.Address
                    });
                }
                return shipmentList;
            }
            return shipmentList;
        }

        public async Task<int> AddAsync(AddShipmentVM shipmentVM)
        {
            if (shipmentVM == null)
                return 0;
            double distance = await _mapService.getKmDistanceAsync(shipmentVM.PickupLocation.Latitude, shipmentVM.PickupLocation.Longitude,
                                                                      shipmentVM.DropLocation.Latitude, shipmentVM.DropLocation.Longitude);
            Shipment shipment = new Shipment()
            {
                Price = calcPrice(shipmentVM.Weight, distance),
                PickupLocation = shipmentVM.PickupLocation,
                DropLocation = shipmentVM.DropLocation,
                Weight = shipmentVM.Weight,
                Description = shipmentVM.Description,
                DeliveredAt = shipmentVM.DeliveredAt

            };
          var res = await _shipmentRepository.AddAsync(shipment);
            if (res != 0)  return 1;
            else return 0;
        }
        public async Task<ShipmentDetailsVM> GetDetailAsync(int? id)
        {
            if (id == null)
                return null;
            var ship = await _shipmentRepository.GetAsync(id);
            if (ship == null)
                return null;

            ShipmentDetailsVM shipmentDetails = new ShipmentDetailsVM()
            {
                Id = ship.Id,
                TrackingNumber = ship.TrackingNumber,
                Location = ship.DropLocation
            };
            return shipmentDetails;
        }
        private decimal calcPrice(double weight, double distance)
        {
            double price = 0;
            price =(weight* distance)/10;
            price *= 5;
            double fees = price * 2 / 100;
            price -= fees;
            return (decimal)( price);
        }

    


      
    }
}
