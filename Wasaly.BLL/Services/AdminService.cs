using System;
using System.Threading.Tasks;
using Wasaly.BLL;
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
                PendingCouriers = pendingCouriers

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
    } }
