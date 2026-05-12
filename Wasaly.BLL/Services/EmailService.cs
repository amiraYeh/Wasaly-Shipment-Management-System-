using MailKit.Security;
using Microsoft.Extensions.Options;
using MimeKit;
using System.Threading.Tasks;
using MailKit.Net.Smtp;
using Wasaly.BLL.Services.Interfaces;
using Wasaly.BLL.Settings;

namespace Wasaly.BLL.Services
{
    public class EmailService : IEmailService
    {
        private readonly EmailSettings _settings;

        public EmailService(IOptions<EmailSettings> settings)
        {
            _settings = settings.Value;
        }

        public async Task SendOtpAsync(string recipientEmail, string recipientName, string otpCode)
        {
            // 1. جهز الإيميل
            var message = new MimeMessage();
            message.From.Add(new MailboxAddress(_settings.SenderName, _settings.SenderEmail));
            message.To.Add(new MailboxAddress(recipientName, recipientEmail));
            message.Subject = "كود تسليم طلبك - وصلني";

            // 2. محتوى الإيميل
            message.Body = new TextPart("html")
            {
                Text = $@"
                <div style='font-family: Arial; text-align: right; direction: rtl;'>
                    <h2>مرحباً {recipientName} 👋</h2>
                    <p>كود تسليم طلبك هو:</p>
                    <h1 style='color: #4CAF50; font-size: 40px; letter-spacing: 10px;'>
                        {otpCode}
                    </h1>
                    <p>الكود صالح لمدة <strong>10 دقائق</strong> فقط</p>
                    <p>أعطِ الكود للمندوب عند استلام طلبك</p>
                    <hr/>
                    <small style='color: gray;'>فريق وصلي 🚚</small>
                </div>"
            };

            // 3. بعت الإيميل
            using var smtp = new SmtpClient();
            await smtp.ConnectAsync(_settings.Host, _settings.Port, SecureSocketOptions.StartTls);
            await smtp.AuthenticateAsync(_settings.SenderEmail, _settings.Password);
            await smtp.SendAsync(message);
            await smtp.DisconnectAsync(true);
        }

        public async Task SendAccountApprovedAsync(string recipientEmail, string recipientName)
        {
            // 1. جهز الإيميل
            var message = new MimeMessage();
            message.From.Add(new MailboxAddress(_settings.SenderName, _settings.SenderEmail));
            message.To.Add(new MailboxAddress(recipientName, recipientEmail));
            message.Subject = "تم توثيق حسابك - وصلني";

            // 2. محتوى الإيميل
            message.Body = new TextPart("html")
            {
                Text = $@"
                <div style='font-family: Arial; text-align: right; direction: rtl;'>
                    <h2>مرحباً {recipientName} 👋</h2>
                    <p>تم قبول وتوثيق حسابك في منصة <strong>وصلني</strong> بنجاح.</p>
                    <p>الآن يمكنك الدخول إلى لوحة التحكم وبدء قبول الشحنات وإدارة مهامك.</p>
                    <p>بالتوفيق 🚚</p>
                    <hr/>
                    <small style='color: gray;'>فريق وصلي</small>
                </div>"
            };

            // 3. بعت الإيميل
            using var smtp = new SmtpClient();
            await smtp.ConnectAsync(_settings.Host, _settings.Port, SecureSocketOptions.StartTls);
            await smtp.AuthenticateAsync(_settings.SenderEmail, _settings.Password);
            await smtp.SendAsync(message);
            await smtp.DisconnectAsync(true);
        }
    }
}
