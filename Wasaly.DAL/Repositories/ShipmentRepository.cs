using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.EntityFrameworkCore;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Wasaly.DAL.Data.Context;
using Wasaly.DAL.Enums;
using Wasaly.DAL.Models;
using Wasaly.DAL.Repositories.IRepositories;

namespace Wasaly.DAL.Repositories
{
    public class ShipmentRepository : IShipmentRepository
    {
        private readonly ApplicationDbContext _context;

        public ShipmentRepository(ApplicationDbContext context)
        {
           _context = context;
        } 
        public async Task<List<Shipment>> GetAllAsync()
        {
                return await _context.Shipments.ToListAsync();        
        } 
        public async Task<Shipment> GetAsync(int? id)
        {
            if (id == null)   return null;
            
            var shipment = await _context.Shipments.FindAsync(id);
            
            if (shipment == null)    return null;

            return shipment;
        }

        public async Task<int> AddAsync(Shipment shipment)
        {
            try
            {
                if (shipment == null)
                    return 0;
                _context.Shipments.Add(shipment);
                _context.SaveChanges();
                return 1;
            }
            catch (Exception ex)
            {
                 new ModelError(ex.Message);
                return 0;
            }
        }
         public async Task UpdateAsync(Shipment shipment)
         {
            try
            {
                if (shipment.Id == null || shipment == null)
                    return;

                var shipmentDB = await _context.Shipments.FindAsync(shipment.Id);

                if (shipmentDB == null) return;

                _context.Shipments.Update(shipment);
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                new ModelError(ex.Message);
                //return 0;
            }
         }
        public async Task DeleteAsync(int? id)
        {
            if (id == null)
                return;

            var shipment = await _context.Shipments.FindAsync(id);
            if (shipment == null) return;

            _context.Shipments.Remove(shipment);
            await _context.SaveChangesAsync();
        }

        public async Task<Tuple<string,int?>> GetCourierData(int? shipmentId)
        {
            if(shipmentId == null)
                return null;
            var courAssigiment = await _context.CourierAssignments.Where(CA => CA.ShipmentId == shipmentId).FirstOrDefaultAsync(c => c.Status == CourierStatus.Accepted);
            if (courAssigiment != null)
            { 
                var name = courAssigiment.Courier.WasalyIdentityUser.FullName;
                var rate = courAssigiment.Courier.WasalyIdentityUser.Rating;
                Tuple<string, int?> res = new Tuple<string, int?>(name, rate);
                return res;
            }
            return null;
        }
    }
}
