using FleetFactory.Application.Interfaces.Services;
using MailKit.Net.Smtp;
using MailKit.Security;
using Microsoft.Extensions.Configuration;
using MimeKit;

namespace FleetFactory.Infrastructure.External;

public class MailkitEmailProvider : IMailService
{
    private readonly IConfiguration _configuration;

    public MailkitEmailProvider(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    public async Task SendEmailAsync(
        string toEmail,
        string subject,
        string body)
    {
        var email = new MimeMessage();

        email.From.Add(
            MailboxAddress.Parse(
                _configuration["MailSettings:Email"]));

        email.To.Add(
            MailboxAddress.Parse(toEmail));

        email.Subject = subject;

        email.Body = new TextPart("plain")
        {
            Text = body
        };

        using var smtp = new SmtpClient();

        await smtp.ConnectAsync(
            _configuration["MailSettings:Host"],
            int.Parse(_configuration["MailSettings:Port"]),
            SecureSocketOptions.StartTls);

        await smtp.AuthenticateAsync(
            _configuration["MailSettings:Email"],
            _configuration["MailSettings:Password"]);

        await smtp.SendAsync(email);

        await smtp.DisconnectAsync(true);
    }
}