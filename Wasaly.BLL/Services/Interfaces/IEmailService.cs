using System.Threading.Tasks;

namespace Wasaly.BLL.Services.Interfaces
{
    public interface IEmailService
    {
        Task EmailAsync(string recipientEmail,string recipientName, string subject,string body);

        Task SendOtpAsync(string recipientEmail,string recipientName,string otpCode);
        Task SendShipmentOnWayEmailAsync(string recipientEmail, string recipientName, string trackingNumber);
        Task SendCourierAcceptedEmailAsync(string recipientEmail,string recipientName);
    }
}
