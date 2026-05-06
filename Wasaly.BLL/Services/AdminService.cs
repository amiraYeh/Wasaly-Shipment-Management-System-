using System;
using System.Threading.Tasks;
using Wasaly.BLL.@interface;
using Wasaly.BLL.ViewModels;
using Wasaly.BLL.ViewModels.AdminVM;
using Wasaly.DAL.Repositories.IRepositories;

namespace Wasaly.BLL.Services
{
    public class AdminService : IAdminService
    {
        private readonly IUserRepository _userRepository;

        public AdminService(IUserRepository userRepository)
        {
            _userRepository = userRepository ?? throw new ArgumentNullException(nameof(userRepository));
        }

        public async Task<AdminStatsVM> GetDashboardStatsAsync()
        {
            var todayCount = await _userRepository.GetTodayShipmentsCountAsync();
            var couriersCount = await _userRepository.GetTotalCouriersCountAsync();
            var merchantsCount = await _userRepository.GetTotalMerchantsCountAsync();
            var pendingCouriers = await _userRepository.GetPendingCouriersAsync();
           


            return new AdminStatsVM
            {
                TodayShipmentsCount = todayCount,
                TotalCouriersCount = couriersCount,
                TotalMerchantsCount = merchantsCount,
                PendingCouriers = pendingCouriers,
                PendingCouriersCount = pendingCouriers.Count(),
                

            };
        }

        // IAdminService.cs - ضيفي


        // AdminService.cs - implementation
        public async Task<CouriersManagementVM> GetCouriersAsync(
            string? search, string? status, string? region)
        {
            var couriers = await _userRepository.GetAllCouriersAsync(search, status, region);

            var rows = couriers
            .Where(c => c.WasalyIdentityUser != null) // ← فلتري الـ null الأول
            .Select(c => new CourierRowVM
            { 
                   Id = c.WasalyIdentityUserId,
                   FullName = c.WasalyIdentityUser.FullName ?? "بدون اسم",
                   PhoneNumber = c.WasalyIdentityUser.PhoneNumber ?? "بدون رقم",
                    Region = c.WasalyIdentityUser.Region.ToString(),
                    IsVerified = c.isVerfied,
                    Rating = c.WasalyIdentityUser.Rating ?? 0,
            }).ToList();
                return new CouriersManagementVM
                {
                   Couriers = rows,
                   TotalCount = rows.Count(),
                   VerifiedCount = rows.Count(c => c.IsVerified),
                   PendingCount = rows.Count(c => !c.IsVerified),
                   SearchTerm = search,
                   StatusFilter = status,
                   RegionFilter = region
                };
        }



        public async Task<bool> UpdateCourierVerificationAsync(string id, bool status)
        {
            return await _userRepository.UpdateCourierStatusAsync(id, status); // مش تكرار
        }


        // AdminService.cs
        public async Task<CourierRowVM?> GetCourierDetailsAsync(string id)
        {
            var courier = await _userRepository.GetCourierByIdAsync(id); // بيرجع Courier
            if (courier == null) return null;

            // هنا بنحول من Courier → CourierRowVM
            return new CourierRowVM
            {
                Id = courier.WasalyIdentityUserId,
                FullName = courier.WasalyIdentityUser.FullName,
                PhoneNumber = courier.WasalyIdentityUser.PhoneNumber,
                Email = courier.WasalyIdentityUser.Email ?? "",
                Region = courier.WasalyIdentityUser.Region.ToString(),
                IsVerified = courier.isVerfied,
                NationalIdImagePath = courier.NationalIdImagePath,
                DrivingLicenseImagePath = courier.DrivingLicenseImagePath,
                ProfileImagePath = courier.ProfileImagePath
            };
        }

        public async Task<MerchantsManagementVM> GetMerchantsAsync(string? search, string? status)

        {
            var merchants = await _userRepository.GetAllMerchantsAsync(search, status);

            var rows = merchants
                .Where(m => m.WasalyIdentityUser != null)
                .Select(m => new MerchantRowVM
                {
                    Id = m.WasalyIdentityUserId,
                    FullName = m.WasalyIdentityUser.FullName,
                    StoreName = m.StoreName,
                    BusinessType = m.BusinessType,
                    PhoneNumber = m.WasalyIdentityUser.PhoneNumber,
                    Region = m.WasalyIdentityUser.Region.ToString(),
                    Email = m.WasalyIdentityUser.Email ?? "",
                    ShipmentsCount = m.shipments?.Count() ?? 0,
                    IsSuspended = false // لو عندك Property للـ Suspend هتحطيها هنا
                }).ToList();
            return new MerchantsManagementVM
            {
                Merchants = rows,
                TotalCount = rows.Count,
                ActiveCount = rows.Count(m => !m.IsSuspended),
                SuspendedCount = rows.Count(m => m.IsSuspended),
                SearchTerm = search,
                StatusFilter = status
            };
        }


        public async Task<MerchantRowVM?> GetMerchantDetailsAsync(string id)
        {
            var merchant = await _userRepository.GetMerchantByIdAsync(id);
            if (merchant == null) return null;

            return new MerchantRowVM
            {
                Id = merchant.WasalyIdentityUserId,
                FullName = merchant.WasalyIdentityUser.FullName,
                StoreName = merchant.StoreName,
                BusinessType = merchant.BusinessType,
                PhoneNumber = merchant.WasalyIdentityUser.PhoneNumber,
                Region = merchant.WasalyIdentityUser.Region.ToString(),
                Email = merchant.WasalyIdentityUser.Email ?? "",
                ShipmentsCount = merchant.shipments?.Count() ?? 0,
                RecentShipments = merchant.shipments?
              .OrderByDescending(s => s.CreatedAt)
               .Take(5)
               .Select(s => new ShipmentRowVM
               {
                   Id = s.TrackingNumber ?? s.Id.ToString(),
                   CourierName = s.CourierAssignments?
                       .FirstOrDefault()?.Courier?
                       .WasalyIdentityUser?.FullName ?? "غير معين",
                   Status = s.Status.ToString(),
                   CreatedAt = s.CreatedAt.ToString("yyyy-MM-dd")
               }) ?? new List<ShipmentRowVM>()

            };

        }
        public async Task<bool> DeleteCourierAsync(string id)
        {
            return await _userRepository.DeleteCourierAsync(id);
        }

        

        // AdminService.cs
        public async Task<bool> DeleteMerchantAsync(string id)
        {
            return await _userRepository.DeleteMerchantAsync(id);
        }
    } 
}
