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
    /// <summary>
    /// Repository for user-related data access operations (couriers, merchants and dashboard stats).
    /// </summary>
    public class UserRepository : IUserRepository
    {
        private readonly ApplicationDbContext _context;

        /// <summary>
        /// Initializes a new instance of <see cref="UserRepository"/>.
        /// </summary>
        /// <param name="context">EF Core <see cref="ApplicationDbContext"/> used for data access.</param>
        public UserRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Retrieves courier details by the linked Wasaly identity user id.
        /// </summary>
        /// <param name="id">Courier unique identifier (Wasaly identity user id).</param>
        /// <returns>The <see cref="Courier"/> if found; otherwise <c>null</c>.</returns>
        public async Task<Courier?> GetCourierByIdAsync(string id)
        {
            return await _context.Couriers
                          .Include(c => c.WasalyIdentityUser)
                         .FirstOrDefaultAsync(c => c.WasalyIdentityUserId == id);
        }

        /// <summary>
        /// Retrieves all couriers that are pending verification.
        /// </summary>
        /// <returns>A collection of unverified <see cref="Courier"/> instances. Returns an empty collection when none exist.</returns>
        // UserRepository.cs - ظبطي GetPendingCouriersAsync
        public async Task<IEnumerable<Courier>> GetPendingCouriersAsync()
        {
            return await _context.Couriers
                                 .Include(c => c.WasalyIdentityUser) 
                                 .Where(c => c.isVerfied == false)
                                 .ToListAsync();
        }
        /// <summary>
        /// Updates the courier verification status.
        /// </summary>
        /// <param name="courierId">Courier unique identifier (primary key).</param>
        /// <param name="status">New verification status to set (true = verified, false = not verified).</param>
        /// <returns><c>true</c> if the update affected the database; otherwise <c>false</c> (e.g. courier not found).</returns>
        public async Task<bool> UpdateCourierStatusAsync(string courierId, bool status)
        {
            var courier = await _context.Couriers.FindAsync(courierId);

            if (courier == null) return false;

            // تحديث خاصية التوثيق بدلاً من الـ Enum
            courier.isVerfied = status;

            // حفظ التغييرات في قاعدة البيانات
            return await _context.SaveChangesAsync() > 0;
        }

        // ================================
        // Dashboard Statistics
        // ================================

        /// <summary>
        /// Retrieves the total number of couriers.
        /// </summary>
        /// <returns>Total courier count.</returns>
        public async Task<int> GetTotalCouriersCountAsync()
        {
            return await _context.Couriers.CountAsync();
        }

        /// <summary>
        /// Retrieves the total number of merchants.
        /// </summary>
        /// <returns>Total merchant count.</returns>
        public async Task<int> GetTotalMerchantsCountAsync()
        {
            return await _context.Merchants.CountAsync();
        }

        /// <summary>
        /// Retrieves the number of shipments created today.
        /// </summary>
        /// <returns>Count of shipments where <see cref="Day9Demo.Models.Shipment.CreatedAt"/> is within today's date range.</returns>
        public async Task<int> GetTodayShipmentsCountAsync()
        {
            var today = DateTime.Today;
            var tomorrow = today.AddDays(1);

            return await _context.Shipments
                                 .Where(s => s.CreatedAt >= today && s.CreatedAt < tomorrow)
                                 .CountAsync();
        }
        public async Task<IEnumerable<Courier>> GetAllCouriersAsync(string? search, string? status, string? region)
        {
            var query = _context.Couriers
                                .Include(c => c.WasalyIdentityUser)
                                .AsQueryable();

            if (!string.IsNullOrEmpty(search))
                query = query.Where(c =>
                    c.WasalyIdentityUser.FullName.Contains(search) ||
                    c.WasalyIdentityUser.PhoneNumber.Contains(search));

            if (status == "موثق")
                query = query.Where(c => c.isVerfied == true);
            else if (status == "انتظار")
                query = query.Where(c => c.isVerfied == false);

            return await query.ToListAsync();
        }
    }
}
