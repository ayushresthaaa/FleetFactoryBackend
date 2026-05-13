using FleetFactory.Application.Features;
using FleetFactory.Application.Features.Mail.DTOs;

namespace FleetFactory.Application.Interfaces.Services
{
    public interface IMailService
    {
      Task SendEmailAsync(SendSalesInvoiceMailRequestDTO request);
    }
}