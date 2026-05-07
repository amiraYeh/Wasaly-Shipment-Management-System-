using System;
using System.Linq;
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

        public async Task<CouriersManagementVM> GetCouriersAsync(
            string? search, string? status, string? region)
        {
            var couriers = await _userRepository.GetAllCouriersAsync(search, status, region);

            var rows = couriers
                .Where(c => c.WasalyIdentityUser != null)
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
            return await _userRepository.UpdateCourierStatusAsync(id, status);
        }

        // Ensure image paths returned are web-accessible.
        private static string MapToWebPath(string? storedPath)
        {
            if (string.IsNullOrWhiteSpace(storedPath))
                return "images/placeholder.png"; // relative to wwwroot

            // If already an absolute URL, return as-is
            if (storedPath.StartsWith("http", StringComparison.OrdinalIgnoreCase))
                return storedPath;

            // If stored path already starts with uploads or slash, normalize without leading slash.
            var trimmed = storedPath.TrimStart('/');

            // If DB stores only filename, assume uploads/couriers folder
            if (!trimmed.Contains("/"))
                return $"uploads/couriers/{trimmed}";

            // Otherwise return trimmed (relative path under wwwroot)
            return trimmed;
        }

        public async Task<CourierRowVM?> GetCourierDetailsAsync(string id)
        {
            var courier = await _userRepository.GetCourierByIdAsync(id); // returns Courier
            if (courier == null) return null;

            return new CourierRowVM
            {
                Id = courier.WasalyIdentityUserId,
                FullName = courier.WasalyIdentityUser.FullName,
                PhoneNumber = courier.WasalyIdentityUser.PhoneNumber,
                Email = courier.WasalyIdentityUser.Email ?? "",
                Region = courier.WasalyIdentityUser.Region.ToString(),
                IsVerified = courier.isVerfied,
                NationalIdImagePath = MapToWebPath(courier.NationalIdImagePath),
                DrivingLicenseImagePath = MapToWebPath(courier.DrivingLicenseImagePath),
                ProfileImagePath = MapToWebPath(courier.ProfileImagePath)
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
                    IsSuspended = false
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

        public async Task<bool> DeleteMerchantAsync(string id)
        {
            return await _userRepository.DeleteMerchantAsync(id);
        }
    }
}
