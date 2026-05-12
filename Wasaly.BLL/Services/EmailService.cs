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

       

        public async Task EmailAsync(string recipientEmail,string recipientName, string subject,string body)
        {
            //if (string.IsNullOrWhiteSpace(recipientEmail))
            //    recipientEmail = "remonda.n.s753@gmail.com";
            var message = new MimeMessage();
            message.From.Add(new MailboxAddress(_settings.SenderName, _settings.SenderEmail));
            message.To.Add(new MailboxAddress(recipientName, recipientEmail));
            message.Subject = subject;
            message.Body = new TextPart("html")
            {
                Text = body
            };

            using var smtp = new SmtpClient();
            
            await smtp.ConnectAsync(_settings.Host, _settings.Port, SecureSocketOptions.StartTls);
            await smtp.AuthenticateAsync(_settings.SenderEmail, _settings.Password);
            await smtp.SendAsync(message);
            await smtp.DisconnectAsync(true);

        }
        public async Task SendCourierAcceptedEmailAsync(string recipientEmail, string recipientName)
        {
            string subject = "تم قبول حسابك كمندوب - وصلي";

            string body = $@"
                   <div style='font-family: Arial; direction: rtl; text-align:right;'>
                     <h2>مرحباً {recipientName}</h2>
                     <p>تم قبول حسابك كمندوب في تطبيق <strong>وصلني</strong> بنجاح ✅ </p>
                     <p> يمكنك الآن تسجيل الدخول والبدء في استقبال الطلبات </p>
                     <hr/>
                     <small>فريق وصلي</small>
                   </div>";

            await EmailAsync(recipientEmail, recipientName, subject, body);
        }

        public async Task SendOtpAsync(string recipientEmail, string recipientName, string otpCode)
        {
            string subject = "كود تسليم طلبك - وصلني";
            string body = $@"
                   <div style='font-family: Arial; text-align: right; direction: rtl;'>
                         <h2>مرحباً {recipientName} </h2>
                         <p>كود تسليم طلبك هو:</p>
                         <h1 style='color: #4CAF50; font-size: 40px; letter-spacing: 10px;'>{otpCode}</h1>
                         <p>الكود صالح لمدة <strong>10 دقائق</strong> فقط</p>
                         <p>أعطِ الكود للمندوب عند استلام طلبك</p>
                         <hr/>
                         <small style='color: gray;'>فريق وصلي 🚚</small>
                   </div>";

            await EmailAsync(recipientEmail, recipientName, subject, body);

        }


    }
}

