using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Wasaly.BLL.ViewModels;
using Wasaly.DAL.Models;
using Wasaly.DAL.Repositories.IRepositories;
using Wasaly.DAL.Enums;

namespace Wasaly.BLL.Services
{
    public class ShipmentService : IShipmentService
    {
        private readonly IShipmentRepository _shipmentRepository;
        private readonly IGoogleMapService _mapService;

        public ShipmentService(IShipmentRepository shipmentRepository, IGoogleMapService mapService)
        {
            _shipmentRepository = shipmentRepository;
            _mapService = mapService;
        }

        public async Task<List<ShipmentVM>> GetShipmentsAsync(string merchantId)
        {
            var shipments = await _shipmentRepository.GetAllAsync(merchantId);

            List<ShipmentVM> shipmentList = new List<ShipmentVM>();

            foreach (var ship in shipments)
            {
                var courdata = ship.CourierAssignments
                    .FirstOrDefault(c => c.ShipmentId == ship.Id);

                shipmentList.Add(new ShipmentVM()
                {
                    Id = ship.Id,
                    TrackingNumber = ship.TrackingNumber,
                    CourierAssignmentName = courdata?.Courier?.WasalyIdentityUser?.FullName
                        ?? "لم تقبل من مندوب بعد",
                    CourierAssignmentRate = courdata?.Courier?.WasalyIdentityUser?.Rating ?? 0,
                    DropLocation = ship.DropLocation.Address,
                    CurrentLatitude = ship.CurrentLatitude,
                    CurrentLongitude = ship.CurrentLongitude,
                    Status = ship.Status,
                    Price = ship.Price
                });
            }

            return shipmentList;
        }

        public async Task<double> AddAsync(AddShipmentVM shipmentVM, string merchantId)
        {
            if (shipmentVM == null)
                return 0;

            double distance = await _mapService.getKmDistanceAsync(
                shipmentVM.PickupLocation.Latitude,
                shipmentVM.PickupLocation.Longitude,
                shipmentVM.DropLocation.Latitude,
                shipmentVM.DropLocation.Longitude
            );

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
                DistanceKm = distance,
                MerchantId = merchantId,
            };

            var res = await _shipmentRepository.AddAsync(shipment);
            return res != 0 ? (double)shipment.Price : 0;
        }

        public async Task<ShipmentDetailsVM> GetDetailAsync(int? id)
        {
            if (id == null)
                return null;

            var shipment = await _shipmentRepository.GetAsync(id);

            if (shipment == null)
                return null;

            return new ShipmentDetailsVM()
            {
                Id = shipment.Id,
                TrackingNumber = shipment.TrackingNumber,
                PickLocation = shipment.PickupLocation,
                DropLocation = shipment.DropLocation,
                Description = shipment.Description,
                Status = shipment.Status,
                DistanceKm = shipment.DistanceKm,
                History = shipment.Trackings.ToList()
            };
        }

        private decimal calcPrice(double weight, double distance)
        {
            double price = (weight * distance) / 10;
            price *= 5;

            double fees = price * 2 / 100;
            price -= fees;

            return (decimal)price;
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
                status = shipment.Status,
            };
        }

        public async Task<MerchantDashboardVM> GetMerchantDataAsync(string id)
        {
            var merchant = await _shipmentRepository.getMerchantData(id);

            if (merchant == null)
                return null;

            var shipments = merchant.shipments.ToList();

            var lastShipments = await GetShipmentsAsync(id);

            var shipmentList = lastShipments.Take(5).ToList();

            var dashboard = new MerchantDashboardVM()
            {
                MerchantName = merchant.WasalyIdentityUser.FullName,

                TodayShipments = merchant.shipments
                    .Count(s => s.CreatedAt.Date == DateTime.UtcNow.Date),

                MonthShipments = merchant.shipments
                    .Count(s => s.CreatedAt.Month == DateTime.UtcNow.Month
                             && s.CreatedAt.Year == DateTime.UtcNow.Year),

                RecentShipments = shipmentList,

                Balance = merchant.Balance,

                TotalPaid = (double)shipments.Sum(s => s.Price)
            };

            return dashboard;
        }
    }
}