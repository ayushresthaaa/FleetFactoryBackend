using FleetFactory.Application.Interfaces.Services;
using FleetFactory.Infrastructure.Helpers;

namespace FleetFactory.Infrastructure.Services
{

    //seer
    public class EmailService(MailKitHelper _mailKitHelper) : IEmailService
    {
        public async Task SendUnpaidCreditReminderAsync(
            string customerEmail,
            string customerName,
            decimal amount)
        {
            string subject = "Action Required: Unpaid Credit Reminder";

            string body = $@"
                <h3>Hello {customerName},</h3>

                <p>
                    This is a reminder that you have an unpaid credit of
                    <b>Rs. {amount}</b>
                    which has been outstanding for more than 1 month.
                </p>

                <p>
                    Please visit Fleet Factory to clear your dues.
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
    }
}