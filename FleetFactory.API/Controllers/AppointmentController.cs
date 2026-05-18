using System.Security.Claims;
using FleetFactory.Application.Features.Appointments.DTOs;
using FleetFactory.Application.Interfaces.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FleetFactory.API.Controllers
{
    [ApiController]
    [Route("api/appointments")]
    public class AppointmentController(
        IAppointmentService _appointmentService
    ) : ControllerBase
    {
        [HttpGet]
        // [Authorize(Roles = "Admin,Staff")]
        public async Task<IActionResult> GetAll(
            [FromQuery] int pageNumber = 1,
            [FromQuery] int pageSize = 10)
        {
            var result = await _appointmentService.GetAllAsync(pageNumber, pageSize);
            return Ok(result);
        }

        [HttpGet("{id:guid}")]
        // [Authorize(Roles = "Admin,Staff")]
        public async Task<IActionResult> GetById(Guid id)
        {
            var result = await _appointmentService.GetByIdAsync(id);

            if (!result.Success)
                return NotFound(result);

            return Ok(result);
        }

        [HttpPost("me")]
        // [Authorize(Roles = "Customer")]
        public async Task<IActionResult> CreateMyAppointment(
            [FromBody] CreateMyAppointmentRequestDTO request)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier)
                ?? User.FindFirstValue("sub");

            if (string.IsNullOrWhiteSpace(userId))
                return Unauthorized();

            var result = await _appointmentService
                .CreateMyAppointmentAsync(userId, request);

            if (!result.Success)
                return BadRequest(result);

            return CreatedAtAction(
                nameof(GetById),
                new { id = result.Data?.Id },
                result
            );
        }

        [HttpPatch("{id:guid}/confirm")]
        // [Authorize(Roles = "Admin,Staff")]
        public async Task<IActionResult> Confirm(Guid id)
        {
            var result = await _appointmentService.ConfirmAsync(id);

            if (!result.Success)
                return BadRequest(result);

            return Ok(result);
        }

        [HttpPatch("{id:guid}/cancel")]
        // [Authorize(Roles = "Admin,Staff")]
        public async Task<IActionResult> Cancel(Guid id)
        {
            var result = await _appointmentService.CancelAsync(id);

            if (!result.Success)
                return BadRequest(result);

            return Ok(result);
        }
    }
}