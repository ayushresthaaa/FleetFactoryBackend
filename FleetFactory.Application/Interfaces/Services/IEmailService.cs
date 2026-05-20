using System.Threading.Tasks;

namespace FleetFactory.Application.Interfaces.Services
{
    public interface IEmailService
    {
  
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

        Task SendOverdueCreditReminderEmailAsync(
            string email,
            string customerName,
            decimal creditBalance
        );
       Task SendLowStockAlertEmailAsync(
            string adminEmail,
            string partName,
            string sku,
            int stockQty,
            int threshold
        );
    }
}