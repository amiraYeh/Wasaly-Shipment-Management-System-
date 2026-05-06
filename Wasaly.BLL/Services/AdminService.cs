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

            return new AdminStatsVM
            {
                TodayShipmentsCount = todayCount,
                TotalCouriersCount = couriersCount,
                TotalMerchantsCount = merchantsCount
            };
        }
    }
}
