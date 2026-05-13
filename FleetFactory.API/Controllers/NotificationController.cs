using FleetFactory.Application.Interfaces.Services;
using FleetFactory.Application.Features.Reports.Services;
using Microsoft.AspNetCore.Mvc;

namespace FleetFactory.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class NotificationController : ControllerBase
    {
        private readonly IEmailService _emailService;
        private readonly IReportService _reportService;

        public NotificationController(IEmailService emailService, IReportService reportService)
        {
            _emailService = emailService;
            _reportService = reportService;
        }

        [HttpPost("send-unpaid-credit-reminders")]
        public async Task<IActionResult> SendReminders()
        {
            var overdueCustomers = await _reportService.GetUnpaidCreditReportAsync();

            if (!overdueCustomers.Any())
            {
                return Ok(new { message = "No customers with overdue credits found." });
            }

            int emailCount = 0;
            foreach (var customer in overdueCustomers)
            {
                // Note: Identity User's Email might be stored elsewhere, 
                // but if you have a way to fetch the email, use it here.
                // Assuming for now you might need to fetch the User email from Identity.
                
                // If you add an Email field to CustomerProfile later, use: customer.Email
                // For now, I'll keep the placeholder for your implementation logic
                string recipientEmail = "user-email-placeholder@example.com"; 

                await _emailService.SendUnpaidCreditReminderAsync(
                    recipientEmail, 
                    customer.FullName, 
                    customer.CreditBalance
                );
                emailCount++;
            }
            
            return Ok(new { 
                message = "Reminder process completed successfully.", 
                remindersSent = emailCount 
            });
        }
    }
}