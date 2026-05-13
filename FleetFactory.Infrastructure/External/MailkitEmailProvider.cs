using FleetFactory.Application.Features;
using FleetFactory.Application.Interfaces.Services;
using FleetFactory.Domain.Entities;
using MailKit.Net.Smtp;
using MailKit.Security;
using Microsoft.Extensions.Options;
using MimeKit;

namespace FleetFactory.Infrastructure.Services
{
    public class MailKitProvider(
        IOptions<MailSetting> _mailSettings)
        : IMailService
    {
        public async Task SendEmailAsync(EmailDto emailDto)
        {
            var email = new MimeMessage();

            // Sender
            email.From.Add(
                new MailboxAddress(
                    _mailSettings.Value.DisplayName,
                    _mailSettings.Value.Email));

            // Receiver
            email.To.Add(
                MailboxAddress.Parse(emailDto.ToEmail));

            // Subject
            email.Subject = emailDto.Subject;

            // Body
            email.Body = new TextPart("html")
            {
                Text = emailDto.Body
            };

            using var smtp = new SmtpClient();

            // Connect SMTP
            await smtp.ConnectAsync(
                _mailSettings.Value.Host,
                _mailSettings.Value.Port,
                SecureSocketOptions.Auto);

            // Login
            await smtp.AuthenticateAsync(
                _mailSettings.Value.Email,
                _mailSettings.Value.Password);

            // Send Mail
            await smtp.SendAsync(email);

            // Disconnect
            await smtp.DisconnectAsync(true);
        }
    }
}