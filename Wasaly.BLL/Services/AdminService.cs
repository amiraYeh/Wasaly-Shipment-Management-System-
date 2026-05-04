using System;
using System.Threading.Tasks;
using Wasaly.BLL;
using Wasaly.BLL.ViewModels;

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
            var todayTask = _userRepository.GetTodayShipmentsCountAsync();
            var couriersTask = _userRepository.GetTotalCouriersCountAsync();
            var merchantsTask = _userRepository.GetTotalMerchantsCountAsync();

            await Task.WhenAll(todayTask, couriersTask, merchantsTask);

            return new AdminStatsVM
            {
                TodayShipmentsCount = await todayTask,
                TotalCouriersCount = await couriersTask,
                TotalMerchantsCount = await merchantsTask
            };
        }
    }
}
