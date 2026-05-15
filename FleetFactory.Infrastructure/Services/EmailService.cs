using FleetFactory.Application.Interfaces.Services;
using FleetFactory.Infrastructure.Helpers;

namespace FleetFactory.Infrastructure.Services
{
    public class EmailService : IEmailService
    {
        public async Task SendUnpaidCreditReminderAsync(string customerEmail, string customerName, decimal amount)
        {
            string subject = "Action Required: Unpaid Credit Reminder";
            string body = $@"
                <h3>Hello {customerName},</h3>
                <p>This is a reminder that you have an unpaid credit of <b>Rs. {amount}</b> 
                which has been outstanding for more than 1 month.</p>
                <p>Please visit Fleet Factory to clear your dues.</p>
                <br/>
                <p>Sent via Fleet Factory System: {DateTimeHelper.NepalNow}</p>";

            await MailKitHelper.SendEmailAsync(customerEmail, subject, body);
        }
    }
}