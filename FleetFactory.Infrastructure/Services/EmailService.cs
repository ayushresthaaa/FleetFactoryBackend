<<<<<<< HEAD
<<<<<<< Updated upstream
=======
=======
>>>>>>> main
using FleetFactory.Application.Interfaces.Services;
using FleetFactory.Infrastructure.Helpers;

namespace FleetFactory.Infrastructure.Services
{
    public class EmailService : IEmailService
    {
<<<<<<< HEAD
        public async Task SendUnpaidCreditReminderAsync(
            string customerEmail,
            string customerName,
            decimal amount)
        {
            string subject = "Unpaid Credit Reminder";

            string body = $@"
                <h3>Hello {customerName}</h3>
                <p>You have an unpaid credit of <b>Rs. {amount}</b> pending for over 1 month.</p>
                <p>Please clear it at Fleet Factory.</p>";
=======
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
>>>>>>> main

            await MailKitHelper.SendEmailAsync(customerEmail, subject, body);
        }
    }
<<<<<<< HEAD
}
>>>>>>> Stashed changes
=======
}
>>>>>>> main
