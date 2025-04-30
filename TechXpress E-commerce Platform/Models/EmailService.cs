using Microsoft.Extensions.Options;
using NETCore.MailKit.Core;
using System.Net.Mail;
using System.Net;
using NETCore.MailKit.Infrastructure.Internal;
using System.Text;

namespace TechXpress_E_commerce_Platform.Models
{
    public class EmailService : IEmailService
    {
        private readonly EmailSettings _emailSettings;

        public EmailService(IOptions<EmailSettings> emailSettings)
        {
            _emailSettings = emailSettings.Value;
        }
        public async Task SendAsync(string toEmail, string subject, string body)
        {
            var email = new MailMessage()
            {
                From = new MailAddress(_emailSettings.SenderEmail, _emailSettings.SenderName),
                Subject = subject,
                Body = body,
                IsBodyHtml = true
            };

            email.To.Add(toEmail);

            using (var client = new SmtpClient(_emailSettings.Server, _emailSettings.Port))
            {
                client.Credentials = new NetworkCredential(_emailSettings.Account, _emailSettings.Password);
                client.EnableSsl = true;

                await client.SendMailAsync(email);
            }
        }

    }

}
