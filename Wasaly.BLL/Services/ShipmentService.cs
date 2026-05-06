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
                var courdata = ship.CourierAssignments.FirstOrDefault(c => c.ShipmentId == ship.Id);
                //if (courdata != null)
                {
                    shipmentList.Add(new ShipmentVM()
                    {
                        Id = ship.Id,
                        TrackingNumber = ship.TrackingNumber,
                        CourierAssignmentName = courdata.Courier.WasalyIdentityUser.FullName,
                        CourierAssignmentRate = courdata.Courier.WasalyIdentityUser.Rating,
                        DropLocation = ship.DropLocation.Address,
                        CurrentLatitude = ship.CurrentLatitude,
                        CurrentLongitude = ship.CurrentLongitude,
                        Status = ship.Status,
                        Price = ship.Price

                    });
                }
                //return shipmentList;
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
                DeliveredAt = shipmentVM.DeliveredAt,
                RecipientEmail = shipmentVM.RecipientEmail,
                RecipientName = shipmentVM.RecipientName,
                DistanceKm = distance
                
            };
          var res = await _shipmentRepository.AddAsync(shipment);
            if (res != 0)  return 1;
            else return 0;
        }
        public async Task<ShipmentDetailsVM> GetDetailAsync(int? id)
        {
            if (id == null)
                return null;
            var shipment = await _shipmentRepository.GetAsync(id);
            if (shipment == null)
                return null;

            List<ShipmentTracking>? history = new List<ShipmentTracking>();
            history = shipment.Trackings.ToList();

            ShipmentDetailsVM shipmentDetails = new ShipmentDetailsVM()
            {
                Id = shipment.Id,
                TrackingNumber = shipment.TrackingNumber,
               PickLocation = shipment.PickupLocation,
               DropLocation = shipment.DropLocation,
               Description = shipment.Description,
               Status = shipment.Status,
               DistanceKm = shipment.DistanceKm,
               History = history
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

        public async Task<Shipment> GetByIDAsync(int id)
        {
           return await _shipmentRepository.GetAsync(id);
        }

        public async Task<int> Update(Shipment shipment)
        {
            return await _shipmentRepository.UpdateAsync(shipment);
        }

        public async Task<object> GetCurrentLocAsync(int id)
        {
            var shipment = await _shipmentRepository.GetAsync(id);
            if (shipment == null)
                return null;
            return new
            {
                lat = shipment.CurrentLatitude,
                lng = shipment.CurrentLongitude,
                status =shipment.Status,
            };
        }
    }
}
