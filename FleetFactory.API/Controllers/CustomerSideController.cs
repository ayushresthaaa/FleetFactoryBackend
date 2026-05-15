using FleetFactory.Application.Interfaces.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace FleetFactory.API.Controllers
{
    [ApiController]
    [Route("api/customer-side")]
    [Authorize (Roles = "Customer")]
    public class CustomerSideController(ICustomerSideService _customerSideService) : ControllerBase
    {
        [HttpGet("me/purchase-history")]
        public async Task<IActionResult> GetMyPurchaseHistory()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier)
                ?? User.FindFirstValue("sub");

            if (string.IsNullOrWhiteSpace(userId))
                return Unauthorized();

            var result = await _customerSideService.GetMyPurchaseHistoryAsync(userId);

            if (!result.Success)
                return NotFound(result);

            return Ok(result);
        }

        [HttpGet("me/appointment-history")]
        public async Task<IActionResult> GetMyAppointmentHistory()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier)
                ?? User.FindFirstValue("sub");

            if (string.IsNullOrWhiteSpace(userId))
                return Unauthorized();

            var result = await _customerSideService.GetMyAppointmentHistoryAsync(userId);

            if (!result.Success)
                return NotFound(result);

            return Ok(result);
        }

        [HttpGet("me/upcoming-appointments")]
        public async Task<IActionResult> GetMyUpcomingAppointments()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier)
                ?? User.FindFirstValue("sub");

            if (string.IsNullOrWhiteSpace(userId))
                return Unauthorized();

            var result = await _customerSideService
                .GetMyUpcomingAppointmentsAsync(userId);

            if (!result.Success)
                return NotFound(result);

            return Ok(result);
        }

        [HttpGet("me/appointments/{appointmentId:guid}")]
        public async Task<IActionResult> GetMyAppointmentById(Guid appointmentId)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier)
                ?? User.FindFirstValue("sub");

            if (string.IsNullOrWhiteSpace(userId))
                return Unauthorized();

            var result = await _customerSideService
                .GetMyAppointmentByIdAsync(userId, appointmentId);

            if (!result.Success)
                return NotFound(result);

            return Ok(result);
        }
    }
}