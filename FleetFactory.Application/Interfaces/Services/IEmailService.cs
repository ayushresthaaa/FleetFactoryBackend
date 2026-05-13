namespace FleetFactory.Application.Interfaces.Services
{
    public interface IEmailService
    {
        Task SendUnpaidCreditReminderAsync(
            string customerEmail,
            string customerName,
            decimal amount);
    }
}