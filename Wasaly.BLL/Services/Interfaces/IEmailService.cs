using System.Threading.Tasks;

namespace Wasaly.BLL.Services.Interfaces
{
    public interface IEmailService
    {
        Task SendOtpAsync(string recipientEmail, string recipientName, string otpCode);
        Task SendAccountApprovedAsync(string recipientEmail, string recipientName);
    }
}
