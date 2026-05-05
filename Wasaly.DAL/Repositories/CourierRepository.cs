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
    public class CourierRepository : ICourierRepository
    {
        private readonly ApplicationDbContext _context;

        public CourierRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<List<Shipment>> GetAvailableShipmentsAsync()
        {
            return await _context.Shipments
                .Where(s => s.Status == ShipmentStatus.Created)
                .Include(s => s.PickupLocation)
                .Include(s => s.DropLocation)
                .Include(s => s.Merchant)
                .ThenInclude(m => m.WasalyIdentityUser)
                .OrderByDescending(s => s.CreatedAt).ToListAsync();
        }
        public async Task AddAssignmentAsync(CourierAssignment assignment)
        {
            await _context.CourierAssignments.AddAsync(assignment);
        }
        public async Task AddTrackingAsync(ShipmentTracking tracking)
        {
            await _context.ShipmentTrackings.AddAsync(tracking);
        }

        public async Task UpdateShipmentStatusAsync(int shipmentId, ShipmentStatus status)
        {
            var shipment=await _context.Shipments.FindAsync(shipmentId);
            if (shipment == null) return;
            shipment.Status=status;
        }

        public async Task<string> GetCourierNameAsync(string courierId)
        {
            var courier = await _context.Couriers
                .Include(c => c.WasalyIdentityUser)
                .FirstOrDefaultAsync(c => c.WasalyIdentityUserId == courierId);

            return courier?.WasalyIdentityUser?.FullName ?? "مندوب";
        }

        public async Task<Shipment?> GetShipmentWithDetailsAsync(int shipmentId)
        {
            return await _context.Shipments!
                .Include(s => s.PickupLocation)
                .Include(s => s.DropLocation)
                .Include(s => s.Merchant)
                    .ThenInclude(m => m.WasalyIdentityUser)
                .Include(s => s.CourierAssignments)
                .Include(s => s.Trackings)
                .FirstOrDefaultAsync(s => s.Id == shipmentId);
        }
        
        public async Task<List<CourierAssignment>> GetCourierAssignmentsAsync(string courierId)
        {
            return await _context.CourierAssignments
                .Where(a => a.CourierId == courierId
                       && a.Status == CourierStatus.Accepted
                       && a.Shipment.Status != ShipmentStatus.Delivered)
                .Include(a => a.Shipment)
                    .ThenInclude(s => s.PickupLocation)
                .Include(a => a.Shipment)
                    .ThenInclude(s => s.DropLocation)
                .Include(a => a.Shipment)
                    .ThenInclude(s => s.Trackings)
                .OrderByDescending(a => a.AssignedAt)
                .ToListAsync();
        }
        public async Task AddOtpAsync(DeliveryOTP otp)
        {
            await _context.DeliveryOTP.AddAsync(otp);
        }
        public Task DeleteOtpAsync(DeliveryOTP otp)
        {
            _context.DeliveryOTP.Remove(otp);
            return Task.CompletedTask;
        }

        public async Task<DeliveryOTP?> GetOtpByShipmentAsync(int shipmentId)
        {
            return await _context.DeliveryOTP
                .FirstOrDefaultAsync(o => o.ShipmentId == shipmentId);
        }

        public async Task UpdateCourierBalanceAsync(string courierId, decimal amount)
        {
            var courier = await _context.Couriers
                .FirstOrDefaultAsync(c => c.WasalyIdentityUserId == courierId);

            if (courier == null) return;

            courier.Balance += amount;
        }

        public async Task SaveAsync()
        {
            await _context.SaveChangesAsync();
        }

        
    }
}
