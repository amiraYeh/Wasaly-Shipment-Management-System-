using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Wasaly.DAL.Models;

namespace Wasaly.DAL.Repositories.IRepositories
{
    public interface IShipmentRepository
    {
        public Task<List<Shipment>> GetAllAsync(string merchantId);
        public Task<Shipment> GetAsync(int? id);
        public Task<int> AddAsync(Shipment shipment);
        public Task<int> UpdateAsync(Shipment shipment);
        public Task DeleteAsync(int? id);

        public Task<Tuple<string, int?, double, double>> GetCourierData(int? shipmentId);
        //public Task<object> getCurrentLoc(int id);

        public Task<Merchant> getMerchantData(string id);

    }
}
