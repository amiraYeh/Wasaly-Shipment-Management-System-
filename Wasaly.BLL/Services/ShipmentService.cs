using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Wasaly.BLL.ViewModels;
using Wasaly.DAL.Repositories;

namespace Wasaly.BLL.Services
{
    public class ShipmentService
    {
        private readonly ShipmentRepository _shipmentRepository;

        public ShipmentService(ShipmentRepository shipmentRepository)
        {
            _shipmentRepository = shipmentRepository;
        }

        public async List<ShipmentVM> GetShipments()
        {
            var shipments = await _shipmentRepository.GetAllAsync();

            List<ShipmentVM> shipmentList = new List<ShipmentVM>();
            foreach (var ship in shipments)
            {
               
                shipmentList.Add(new ShipmentVM
                {
                    TrackingNumber = ship.TrackingNumber,


                });
                return shipmentList;
            }
        }
    }
}
