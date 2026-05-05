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
    public class UserRepository : IUserRepository
    {
        private readonly ApplicationDbContext _context;

        public UserRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<Courier> GetCourierByIdAsync(string id)
        {
            return await _context.Couriers
                         .FirstOrDefaultAsync(c => c.WasalyIdentityUserId == id);
        }

        public async Task<IEnumerable<Courier>> GetPendingCouriersAsync()
        {
            // المندوب اللي لسه مخلصش مرحلة التوثيق
            return await _context.Couriers
                                 .Where(c => c.isVerfied == false)
                                 .ToListAsync();
        }
      

        public async Task<bool> UpdateCourierStatusAsync(string courierId, bool isVerified)
        {
            var courier = await _context.Couriers.FindAsync(courierId);

            if (courier == null) return false;

            // تحديث خاصية التوثيق بدلاً من الـ Enum
            courier.isVerfied = isVerified;

            // حفظ التغييرات في قاعدة البيانات
            return await _context.SaveChangesAsync() > 0;
        }
    }
}
