


using FleetFactory.Application.Interfaces.Repositories;
using FleetFactory.Application.Interfaces.Services;

namespace FleetFactory.Application.Features.Mail.Services
{
    public class SalesInvoiceEmailService : ISalesInvoiceEmailService
    {
        private readonly ISalesEmailRepository _repository;
    
        private readonly IMailService _mailService;

        public SalesInvoiceEmailService(
            ISalesEmailRepository repository,
            IMailService mailService)
        {
            _repository = repository;
            _mailService = mailService;
        }

    public async Task<bool> SendSalesInvoiceMailAsync(Guid salesInvoiceId)
    {
        var invoice = await _repository.GetInvoiceWithCustomerAsync(salesInvoiceId);

        if (invoice == null)
        {
            return false;
        }

        var customerEmail = invoice.Customer.Email;

        var body = $@"
            Dear {invoice.Customer.FullName},

            Your sales invoice has been generated successfully.

            Invoice Number: {invoice.InvoiceNumber}
            Total Amount: {invoice.TotalAmount}

            Thank you for visiting FleetFactory.
        ";

        await _mailService.SendEmailAsync(
            customerEmail,
            "Sales Invoice",
            body);

        return true;
    }
}

}
