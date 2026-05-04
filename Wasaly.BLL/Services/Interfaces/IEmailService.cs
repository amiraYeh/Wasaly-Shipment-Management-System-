using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Wasaly.BLL.Services.Interfaces
{
    public interface IEmailService
    {
        Task SendOtpAsync(string recipientEmail, string recipientName, string otpCode);
    }
}
