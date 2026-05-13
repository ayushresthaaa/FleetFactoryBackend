namespace FleetFactory.Application.Interfaces.Services
{
    public interface ISalesInvoiceEmailService
{
    Task<bool> SendSalesInvoiceMailAsync(Guid salesInvoiceId);
}
}

