using MailKit.Net.Smtp;
using MimeKit;

namespace FleetFactory.Shared.Helpers
{
    
    public static class MailKitHelper 
    {
        private static readonly string _smtpServer = "your-smtp-server.com";
        private static readonly int _port = 587;
        private static readonly string _username = "your-email@domain.com";
        private static readonly string _password = "your-password";

        public static async Task SendEmailAsync(string toEmail, string subject, string body)
        {
            var message = new MimeMessage();
            message.From.Add(new MailboxAddress("Fleet Factory", _username));
            message.To.Add(new MailboxAddress("", toEmail));
            message.Subject = subject;

            message.Body = new TextPart("html") { Text = body };

            using var client = new SmtpClient();
            await client.ConnectAsync(_smtpServer, _port, MailKit.Security.SecureSocketOptions.StartTls);
            await client.AuthenticateAsync(_username, _password);
            await client.SendAsync(message);
            await client.DisconnectAsync(true);
        }
    }
}