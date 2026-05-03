using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Wasaly.DAL.Data.Context;
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

        public async Task AddAsync(Shipment shipment)
        {
            try
            {
                if (shipment == null)
                    return;
                await _context.Shipments.AddAsync(shipment);
            }
            catch (Exception ex)
            {
                 new ModelError(ex.Message);
            }
        }
         public async Task UpdateAsync(Shipment shipment)
         {
            if (shipment.Id == null || shipment == null)
                return;

            var shipmentDB = await _context.Shipments.FindAsync(shipment.Id);

            if (shipmentDB == null) return;

            _context.Shipments.Update(shipment);
           await _context.SaveChangesAsync();
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

        public string GetCourierName(int? id)
        {
            //var courierAssignment =  _context.Shipments.FirstOrDefault(s=>s.CourierAssignment.Id == id);
            //var courierAssignment =  _context.Couriers.FirstOrDefault(c=>c. == id);


            //if (courierAssignment == null)
                //return null;
            //return courierAssignment.FullName;
            return "";
        }

        Task<string> IShipmentRepository.GetCourierName(int? id)
        {
            throw new NotImplementedException();
        }
    }
}
