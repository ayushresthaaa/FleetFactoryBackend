using FleetFactory.Application.Features;

namespace FleetFactory.Application.Interfaces.Services
{
    public interface IMailService
    {
      Task SendEmailAsync(EmailDto emailDto);
    }
}