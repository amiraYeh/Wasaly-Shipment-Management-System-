using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Wasaly.BLL.ViewModels;

namespace Wasaly.BLL.Services.Interfaces
{
    public interface ICourierService
    {
        Task<List<AvailableShipmentVM>> GetAvailableShipmentsAsync();
        Task<bool> AcceptShipmentAsync(int shipmentId, string courierId);
        Task<bool> PickupShipmentAsync(int shipmentId, string courierId);

        Task<List<CourierShipmentVM>> GetCourierShipmentsAsync(string courierId);

        Task<bool> GenerateAndSendOtpAsync(int shipmentId);
        Task<(bool Success, string Message)> VerifyOtpAndDeliverAsync(VerifyOtpVM model, string courierId);
    }
}
