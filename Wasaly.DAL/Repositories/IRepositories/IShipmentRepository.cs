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
        public Task<List<Shipment>> GetAllAsync();
        public Task<Shipment> GetAsync(int? id);
        public Task AddAsync(Shipment shipment);
        public Task UpdateAsync(Shipment shipment);
        public Task DeleteAsync(int? id);

        public Task<string> GetCourierName(int? id);
    }
}
