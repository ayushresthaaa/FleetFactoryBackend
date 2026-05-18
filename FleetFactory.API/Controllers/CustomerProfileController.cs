using System.Security.Claims;
using FleetFactory.Application.Features.CustomerProfileManagement.DTOs;
using FleetFactory.Application.Interfaces.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FleetFactory.API.Controllers
{
    [ApiController]
    [Route("api/customer-profile")]
    [Authorize(Roles = "Customer")]
    public class CustomerProfileController(
        ICustomerProfileService _customerProfileService
    ) : ControllerBase
    {
        [HttpGet("me")]
        public async Task<IActionResult> GetMyProfile()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier)
                ?? User.FindFirstValue("sub");

            if (string.IsNullOrWhiteSpace(userId))
                return Unauthorized();

            var result = await _customerProfileService
                .GetMyProfileAsync(userId);

            if (!result.Success)
                return NotFound(result);

            return Ok(result);
        }

        [HttpPut("me")]
        public async Task<IActionResult> UpdateMyProfile(
            [FromBody] UpdateMyProfileRequestDto request)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier)
                ?? User.FindFirstValue("sub");

            if (string.IsNullOrWhiteSpace(userId))
                return Unauthorized();

            var result = await _customerProfileService
                .UpdateMyProfileAsync(userId, request);

            if (!result.Success)
                return BadRequest(result);

            return Ok(result);
        }

        [HttpPost("me/vehicles")]
        public async Task<IActionResult> AddMyVehicle(
            [FromBody] AddMyVehicleRequestDto request)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier)
                ?? User.FindFirstValue("sub");

            if (string.IsNullOrWhiteSpace(userId))
                return Unauthorized();

            var result = await _customerProfileService
                .AddMyVehicleAsync(userId, request);

            if (!result.Success)
                return BadRequest(result);

            return Ok(result);
        }

        [HttpPut("me/vehicles/{vehicleId:guid}")]
        public async Task<IActionResult> UpdateMyVehicle(
            Guid vehicleId,
            [FromBody] UpdateMyVehicleRequestDto request)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier)
                ?? User.FindFirstValue("sub");

            if (string.IsNullOrWhiteSpace(userId))
                return Unauthorized();

            var result = await _customerProfileService
                .UpdateMyVehicleAsync(userId, vehicleId, request);

            if (!result.Success)
                return BadRequest(result);

            return Ok(result);
        }

        [HttpDelete("me/vehicles/{vehicleId:guid}")]
        public async Task<IActionResult> DeleteMyVehicle(Guid vehicleId)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier)
                ?? User.FindFirstValue("sub");

            if (string.IsNullOrWhiteSpace(userId))
                return Unauthorized();

            var result = await _customerProfileService
                .DeleteMyVehicleAsync(userId, vehicleId);

            if (!result.Success)
                return BadRequest(result);

            return Ok(result);
        }
    }
}