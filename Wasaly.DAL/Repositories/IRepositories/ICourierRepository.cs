using Wasaly.DAL.Enums;
using Wasaly.DAL.Models;

namespace Wasaly.DAL.Repositories.IRepositories
{
    public interface ICourierRepository
    {
        Task<List<Shipment>> GetAvailableShipmentsAsync();
        Task<List<CourierAssignment>> GetCourierAssignmentsAsync(string courierId);
        Task<Shipment?> GetShipmentWithDetailsAsync(int shipmentId);
        Task AddAssignmentAsync(CourierAssignment assignment);
        Task AddTrackingAsync(ShipmentTracking tracking);
        Task UpdateShipmentStatusAsync(int shipmentId, ShipmentStatus status);
        Task<string> GetCourierNameAsync(string courierId);
        Task AddOtpAsync(DeliveryOTP otp);
        Task DeleteOtpAsync(DeliveryOTP otp);
        Task<DeliveryOTP?> GetOtpByShipmentAsync(int shipmentId);
        Task UpdateCourierBalanceAsync(string courierId, decimal amount);
        Task SaveAsync();
    }
}