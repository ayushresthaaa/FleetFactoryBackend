using FleetFactory.Application.Interfaces.Services;
using Microsoft.AspNetCore.Mvc;

namespace FleetFactory.API.Controllers
{
    [ApiController]
    [Route("api/overdue-credits")]
    public class OverdueCreditController(
        IOverdueCreditService _overdueCreditService
    ) : ControllerBase
    {
        [HttpGet]
        public async Task<IActionResult> GetOverdueCredits()
        {
            var result = await _overdueCreditService.GetOverdueCreditsAsync();
            return Ok(result);
        }

        [HttpPost("{customerId:guid}/send-reminder")]
        public async Task<IActionResult> SendReminderToCustomer(Guid customerId)
        {
            var result = await _overdueCreditService.SendReminderToCustomerAsync(customerId);

            if (!result.Success)
                return BadRequest(result);

            return Ok(result);
        }

        [HttpPost("send-reminders")]
        public async Task<IActionResult> SendReminderToAll()
        {
            var result = await _overdueCreditService.SendReminderToAllAsync();

            if (!result.Success)
                return BadRequest(result);

            return Ok(result);
        }
    }
}