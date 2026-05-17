using FleetFactory.Shared.Results;

namespace FleetFactory.Application.Interfaces.Services
{
    public interface ISendInvoiceEmailService
    {
        Task<ApiResponse<string>> SendSalesInvoiceEmailAsync(Guid invoiceId);
    }
}