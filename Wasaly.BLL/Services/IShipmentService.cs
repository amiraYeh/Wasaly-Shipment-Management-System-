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
        public Task<List<ShipmentVM>> GetShipmentsAsync();
        public Task<ShipmentDetailsVM> GetDetailAsync(int? id);
        public Task<int> AddAsync(AddShipmentVM shipmentVM);
        //public double calDistance(Location start, Location end);
    }
}
