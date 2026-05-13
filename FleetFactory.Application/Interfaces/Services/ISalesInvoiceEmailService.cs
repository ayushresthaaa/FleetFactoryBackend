using FleetFactory.Domain.Entities;

namespace FleetFactory.Application.Interfaces.Services
{
    public interface ISalesInvoiceEmailService
    {
        Task <bool> SendSalesInvoiceEmailAsync(SendSalesInvoiceMailDto sendSalesInvoiceMailDto);
    }
}