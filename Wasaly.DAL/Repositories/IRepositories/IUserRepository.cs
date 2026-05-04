using Day9Demo.Models;

namespace Wasaly.DAL.Repositories.IRepositories
{
    public interface IUserRepository
    {

        /// <summary>
        /// Retrieves all couriers waiting for verification.
        /// </summary>
        Task<IEnumerable<Courier>> GetPendingCouriersAsync();

        /// <summary>
        /// Updates courier verification status (Approved / Rejected).
        /// </summary>
        /// <param name="courierId">Courier unique identifier.</param>
        /// <param name="status">New verification status.</param>
        Task<bool> UpdateCourierStatusAsync(string courierId, bool status);

        /// <summary>
        /// Retrieves courier details by id.
        /// </summary>
        /// <param name="id">Courier unique identifier.</param>
        Task<Courier?> GetCourierByIdAsync(string id);



        // ================================
        // Dashboard Statistics
        // ================================

        /// <summary>
        /// Retrieves total number of couriers.
        /// </summary>
        Task<int> GetTotalCouriersCountAsync();

        /// <summary>
        /// Retrieves total number of merchants.
        /// </summary>
        Task<int> GetTotalMerchantsCountAsync();

        /// <summary>
        /// Retrieves total shipments created today.
        /// </summary>
        Task<int> GetTodayShipmentsCountAsync();


    }
}
