using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using FleetFactory.Application.Interfaces.Services;
using FleetFactory.Domain.Entities;
using FleetFactory.Infrastructure.Identity;

namespace FleetFactory.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class NotificationController : ControllerBase
    {
        private readonly IReportService _reportService;
        private readonly IEmailService _emailService;
        private readonly UserManager<ApplicationUser> _userManager;

        public NotificationController(
            IReportService reportService,
            IEmailService emailService,
            UserManager<ApplicationUser> userManager)
        {
            _reportService = reportService;
            _emailService = emailService;
            _userManager = userManager;
        }

        [HttpPost("send-overdue-credit-emails")]
        public async Task<IActionResult> SendOverdueEmails()
        {
            var customers = await _reportService.GetUnpaidCreditReportAsync();

            if (customers == null || !customers.Any())
                return Ok(new { message = "No overdue customers found." });

            int sentCount = 0;

            foreach (var customer in customers)
            {
                
                var user = await _userManager.FindByIdAsync(customer.UserId);

                if (user?.Email is null || string.IsNullOrWhiteSpace(user.Email))
                    continue;

                await _emailService.SendUnpaidCreditReminderAsync(
                    user.Email,
                    customer.FullName,
                    customer.CreditBalance
                );

                sentCount++;
            }

            return Ok(new
            {
                message = "Overdue credit emails sent successfully",
                totalSent = sentCount
            });
        }
    }
}