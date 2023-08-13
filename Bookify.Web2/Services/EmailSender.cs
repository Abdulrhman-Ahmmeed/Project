using Bookify.Web2.setting;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.Extensions.Options;
using System.Net;
using System.Net.Mail;
namespace Bookify.Web2.Services
{
    public class EmailSender : IEmailSender
    {
        private readonly MailSettings _mailSettings;
        private readonly IWebHostEnvironment _web;
        public EmailSender(IOptions<MailSettings> mailSettings, IWebHostEnvironment web)
        {
            _mailSettings = mailSettings.Value;
            _web = web;
        }
        public async Task SendEmailAsync(string email, string subject, string htmlMessage)
        {
            MailMessage message = new MailMessage()
            {
                From = new MailAddress(_mailSettings.Email!, _mailSettings.DisplayName),
                Subject = subject,
                Body = htmlMessage,
                IsBodyHtml = true,
            };
            message.To.Add(_web.IsDevelopment()?"abdelrhmanbebo68@gmail.com":email);
            SmtpClient smtp = new(_mailSettings.Host,_mailSettings.Port)
            {
                Port = _mailSettings.Port,
                Credentials = new NetworkCredential(_mailSettings.Email, _mailSettings.Password),
                EnableSsl = true
            };
            await smtp.SendMailAsync(message);
            smtp.Dispose();
        }
    }
}
