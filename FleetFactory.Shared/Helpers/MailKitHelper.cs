using MailKit.Net.Smtp;
using MimeKit;
using Microsoft.Extensions.Configuration;

namespace FleetFactory.Infrastructure.Helpers
{
    public class MailKitHelper
    {
        private readonly IConfiguration _configuration;

        public MailKitHelper(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public async Task SendEmailAsync(
            string toEmail,
            string subject,
            string body)
        {
            var smtpServer = _configuration["EmailSettings:SmtpServer"];
            var port = int.Parse(_configuration["EmailSettings:Port"]!);
            var username = _configuration["EmailSettings:Username"];
            var password = _configuration["EmailSettings:Password"];

            var message = new MimeMessage();

            message.From.Add(
                new MailboxAddress("Fleet Factory", username)
            );

            message.To.Add(
                MailboxAddress.Parse(toEmail)
            );

            message.Subject = subject;

            message.Body = new TextPart("html")
            {
                Text = body
            };

            using var client = new SmtpClient();

            await client.ConnectAsync(
                smtpServer,
                port,
                MailKit.Security.SecureSocketOptions.StartTls
            );

            await client.AuthenticateAsync(
                username,
                password
            );

            await client.SendAsync(message);

            await client.DisconnectAsync(true);
        }
    }
}