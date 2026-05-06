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
            return await _context.Shipments.Include(s => s.DropLocation).Include(s => s.CourierAssignments).ThenInclude(c=>c.Courier).ThenInclude(cc=>cc.WasalyIdentityUser).ToListAsync();        
        } 
        public async Task<Shipment> GetAsync(int? id)
        {
            if (id == null)   return null;
            
            var shipment = await _context.Shipments.Include(s=>s.Trackings).Include(s=>s.PickupLocation).Include(s=>s.DropLocation).FirstOrDefaultAsync(s=>s.Id ==id);
            
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
         public async Task<int> UpdateAsync(Shipment shipment)
         {
            try
            {
                if (shipment.Id == null || shipment == null)
                    return 0;

                var shipmentDB = await _context.Shipments.FindAsync(shipment.Id);

                if (shipmentDB == null) return 0;

                _context.Shipments.Update(shipment);
                await _context.SaveChangesAsync();
                return 1;
            }
            catch (Exception ex)
            {
                new ModelError(ex.Message);
                return 0;
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

        public async Task<Tuple<string, int?, double, double>> GetCourierData(int? shipmentId)
        {
            if(shipmentId == null)
                return null;
            //var courAssigiment = await _context.CourierAssignments.Where(CA => CA.ShipmentId == shipmentId).FirstOrDefaultAsync(c => c.Status == CourierStatus.Accepted);

            var assignment = _context.CourierAssignments.Include(a => a.Shipment).Include(a=>a.Courier).FirstOrDefault();
            if (assignment != null)
            {
                var cour = await _context.Couriers.Include(c=>c.WasalyIdentityUser).FirstOrDefaultAsync(c=>c.WasalyIdentityUserId ==assignment.CourierId);
              //  return await _context.CourierAssignments
              //.Where(a => a.CourierId == courierId
              //       && a.Status == CourierStatus.Accepted
              //       && a.Shipment.Status != ShipmentStatus.Delivered)
              //.Include(a => a.Shipment)
              //    .ThenInclude(s => s.PickupLocation)
              //.Include(a => a.Shipment)
              //    .ThenInclude(s => s.DropLocation)
              //.Include(a => a.Shipment)
              //    .ThenInclude(s => s.Trackings)
              //.OrderByDescending(a => a.AssignedAt)
              //.ToListAsync();
                //var courier = _context.Users.Find(courAssigiment.CourierId);
                var name = cour.WasalyIdentityUser.FullName;
                var rate = cour.WasalyIdentityUser.Rating;
                
                double lng = 33.5;
                double lat = 20.5;
                //var rate = courAssigiment.Courier.WasalyIdentityUser.Rating;
                //double lat = courier.Location.Latitude;
                //double lat = courAssigiment.Courier.WasalyIdentityUser.Location.Latitude;
                //double lng = courier.Location.Longitude;
                //double lng = courAssigiment.Courier.WasalyIdentityUser.Location.Longitude;
                Tuple<string, int?,double,double> res = new Tuple<string, int?,double,double>(name, rate,lat,lng);
                return res;
            }
            return null;
        }

        public Task<object> getCurrentLoc(int id)
        {
            throw new NotImplementedException();
        }
    }
}
