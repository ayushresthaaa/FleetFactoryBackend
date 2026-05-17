using FleetFactory.Application.Interfaces.Repositories;
using FleetFactory.Application.Interfaces.Services;
using FleetFactory.Shared.Results;

namespace FleetFactory.Application.Features.SalesInvoices.Services
{
    public class SendInvoiceEmailService(
        ISalesInvoiceRepository _salesInvoiceRepository,
        IEmailService _emailService
    ) : ISendInvoiceEmailService
    {
        public async Task<ApiResponse<string>> SendSalesInvoiceEmailAsync(Guid invoiceId)
        {
            var invoice = await _salesInvoiceRepository.GetByIdAsync(invoiceId);

            if (invoice == null)
            {
                return ApiResponse<string>
                    .ErrorResponse("Sales invoice not found");
            }

            var customerEmail = invoice.Customer?.User?.Email;

            var customerName = invoice.Customer?.FullName ?? "Customer";

            if (string.IsNullOrWhiteSpace(customerEmail))
            {
                return ApiResponse<string>
                    .ErrorResponse("Customer email not found");
            }

            await _emailService.SendSalesInvoiceAsync(
                customerEmail,
                customerName,
                invoice.InvoiceNo,
                invoice.TotalAmount
            );

            return ApiResponse<string>
                .SuccessResponse(
                    "Email sent",
                    "Sales invoice email sent successfully"
                );
        }
    }
}