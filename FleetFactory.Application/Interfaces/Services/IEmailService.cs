using System.Threading.Tasks;

namespace FleetFactory.Application.Interfaces.Services
{
    public interface IEmailService
    {
        Task SendUnpaidCreditReminderAsync(string customerEmail, string customerName, decimal amount);

        Task SendSalesInvoiceAsync(
            string customerEmail,
            string customerName,
            string invoiceNo,
            decimal totalAmount
        );
        Task SendPartRequestSourcedEmailAsync(
            string customerEmail,
            string customerName,
            string partName);
        Task SendAppointmentCancelledEmailAsync(
            string customerEmail,
            string customerName,
            DateTime scheduledAt,
            string? reason);

       Task SendLowStockAlertEmailAsync(
            string partName,
            string sku,
            int stockQty,
            int threshold);
        
    }
}