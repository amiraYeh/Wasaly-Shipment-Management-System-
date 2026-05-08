using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Wasaly.BLL.ViewModels;
using Wasaly.DAL.Models;

namespace Wasaly.BLL.Services
{
    public interface IShipmentService
    {
        public Task<List<ShipmentVM>> GetShipmentsAsync(string merchantId);
        public Task<ShipmentDetailsVM> GetDetailAsync(int? id);
        public Task<double> AddAsync(AddShipmentVM shipmentVM,string merchantId);
        public Task<Shipment>GetByIDAsync(int id);
        public Task<int> Update(Shipment shipment);
        public Task<object> GetCurrentLocAsync(int id);
        public Task<MerchantDashboardVM> GetMerchantDataAsync(string id);
        //public double calDistance(Location start, Location end);
    }
}
