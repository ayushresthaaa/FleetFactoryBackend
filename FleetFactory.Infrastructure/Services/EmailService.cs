using FleetFactory.Application.Interfaces.Services;
using FleetFactory.Infrastructure.Helpers;


namespace FleetFactory.Infrastructure.Services
{

    //seer
    public class EmailService(MailKitHelper _mailKitHelper) : IEmailService
    {

        //rabison : to send sales invoice email to customer
        public async Task SendSalesInvoiceAsync(
            string customerEmail,
            string customerName,
            string invoiceNo,
            decimal totalAmount)
        {
            string subject = $"Fleet Factory Invoice - {invoiceNo}";

            string body = $@"
                <h2>Fleet Factory Invoice</h2>

                <p>Hello {customerName},</p>

                <p>Your sales invoice has been generated successfully.</p>

                <table style='border-collapse: collapse; width: 100%;'>
                    <tr>
                        <td style='padding: 8px; border: 1px solid #ddd;'><b>Invoice No</b></td>
                        <td style='padding: 8px; border: 1px solid #ddd;'>{invoiceNo}</td>
                    </tr>
                    <tr>
                        <td style='padding: 8px; border: 1px solid #ddd;'><b>Total Amount</b></td>
                        <td style='padding: 8px; border: 1px solid #ddd;'>Rs. {totalAmount}</td>
                    </tr>
                </table>

                <br/>

                <p>Thank you for choosing Fleet Factory.</p>

                <p>Sent via Fleet Factory System: {DateTimeHelper.NepalNow}</p>";

            await _mailKitHelper.SendEmailAsync(
                customerEmail,
                subject,
                body
            );
        }
        
        //rabisoon to send unpaid credit reminder email to customer
        public async Task SendOverdueCreditReminderEmailAsync(
            string email,
            string customerName,
            decimal creditBalance)
        {
            string subject = "Fleet Factory - Overdue Credit Reminder";

            string body = $@"
                <h2>Overdue Credit Reminder</h2>

                <p>Hello {customerName},</p>

                <p>
                    This is a reminder that you have an unpaid credit balance
                    with Fleet Factory.
                </p>

                <table style='border-collapse: collapse; width: 100%;'>
                    <tr>
                        <td style='padding: 8px; border: 1px solid #ddd;'>
                            <b>Pending Credit Amount</b>
                        </td>
                        <td style='padding: 8px; border: 1px solid #ddd;'>
                            Rs. {creditBalance}
                        </td>
                    </tr>
                </table>

                <br/>

                <p>
                    Please clear your pending payment as soon as possible.
                </p>

                <p>
                    If you have already paid, please ignore this message.
                </p>

                <br/>

                <p>
                    Sent via Fleet Factory System:
                    {DateTimeHelper.NepalNow}
                </p>";

            await _mailKitHelper.SendEmailAsync(
                email,
                subject,
                body
            );
        }

        //part request sourced email rachina
        public async Task SendPartRequestSourcedEmailAsync(
            string customerEmail,
            string customerName,
            string partName)
        {
            string subject = $"Requested Part Available - {partName}";

            string body = $@"
                <h2>Requested Part Available</h2>

                <p>Hello {customerName},</p>

                <p>
                    The part you requested:
                    <b>{partName}</b>
                    is now available at Fleet Factory.
                </p>

                <p>
                    Please visit our store for further details.
                </p>

                <br/>

                <p>
                    Sent via Fleet Factory System:
                    {DateTimeHelper.NepalNow}
                </p>";

            await _mailKitHelper.SendEmailAsync(
                customerEmail,
                subject,
                body
            );
        }


        public async Task SendAppointmentCancelledEmailAsync(
            string customerEmail,
            string customerName,
            DateTime scheduledAt,
            string? reason)
        {
            string subject = "Fleet Factory Appointment Cancelled";

            string body = $@"
                <h2>Appointment Cancelled</h2>

                <p>Hello {customerName},</p>

                <p>
                    Your appointment scheduled for
                    <b>{scheduledAt}</b>
                    has been cancelled.
                </p>

                <p>
                    <b>Reason:</b> {(string.IsNullOrWhiteSpace(reason) ? "Not specified" : reason)}
                </p>

                <br/>

                <p>
                    Sent via Fleet Factory System:
                    {DateTimeHelper.NepalNow}
                </p>";

            await _mailKitHelper.SendEmailAsync(
                customerEmail,
                subject,
                body
            );
        }
      //seer : to send low stock alert email to admin
        public async Task SendLowStockAlertEmailAsync(
            string adminEmail,
            string partName,
            string sku,
            int stockQty,
            int threshold)
        {
            string subject = $"Low Stock Alert - {partName}";

            string body = $@"
                <h2>Low Stock Alert</h2>

                <p>
                    The following part is low in stock:
                </p>

                <table style='border-collapse: collapse; width: 100%;'>
                    <tr>
                        <td><b>Part</b></td>
                        <td>{partName}</td>
                    </tr>
                    <tr>
                        <td><b>SKU</b></td>
                        <td>{sku}</td>
                    </tr>
                    <tr>
                        <td><b>Current Stock</b></td>
                        <td>{stockQty}</td>
                    </tr>
                    <tr>
                        <td><b>Threshold</b></td>
                        <td>{threshold}</td>
                    </tr>
                </table>

                <br/>

                <p>
                    Please restock this item as soon as possible.
                </p>

                <p>
                    Sent via Fleet Factory System:
                    {DateTimeHelper.NepalNow}
                </p>";

            await _mailKitHelper.SendEmailAsync(
                adminEmail,
                subject,
                body
            );
        }
    }
}