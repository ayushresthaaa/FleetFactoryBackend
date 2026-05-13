<<<<<<< Updated upstream
=======
using FleetFactory.Application.Interfaces.Services;
using FleetFactory.Infrastructure.Helpers;

namespace FleetFactory.Infrastructure.Services
{
    public class EmailService : IEmailService
    {
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

            await MailKitHelper.SendEmailAsync(customerEmail, subject, body);
        }
    }
}
>>>>>>> Stashed changes
