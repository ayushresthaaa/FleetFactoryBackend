using System.Security.Claims;
using FleetFactory.Application.Features.PartRequests.DTOs;
using FleetFactory.Application.Interfaces.Services;
using FleetFactory.Domain.Enums;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FleetFactory.API.Controllers
{
    [ApiController]
    [Route("api/part-requests")]
    public class PartRequestsController(
        IPartRequestService _partRequestService
    ) : ControllerBase
    {
        [HttpPost("me")]
        [Authorize(Roles = "Customer")]
        public async Task<IActionResult> CreateMyRequest(
            [FromBody] CreatePartRequestRequestDTO request)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier)
                ?? User.FindFirstValue("sub");

            if (string.IsNullOrWhiteSpace(userId))
                return Unauthorized();

            var result = await _partRequestService.CreateMyRequestAsync(userId, request);

            if (!result.Success)
                return BadRequest(result);

            return Ok(result);
        }

        [HttpGet("me")]
        [Authorize(Roles = "Customer")]
        public async Task<IActionResult> GetMyRequests()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier)
                ?? User.FindFirstValue("sub");

            if (string.IsNullOrWhiteSpace(userId))
                return Unauthorized();

            var result = await _partRequestService.GetMyRequestsAsync(userId);

            if (!result.Success)
                return NotFound(result);

            return Ok(result);
        }

        [HttpGet("me/{id:guid}")]
        [Authorize(Roles = "Customer")]
        public async Task<IActionResult> GetMyRequestById(Guid id)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier)
                ?? User.FindFirstValue("sub");

            if (string.IsNullOrWhiteSpace(userId))
                return Unauthorized();

            var result = await _partRequestService.GetMyRequestByIdAsync(userId, id);

            if (!result.Success)
                return NotFound(result);

            return Ok(result);
        }

        [HttpGet]
        [Authorize(Roles = "Admin,Staff")]
        public async Task<IActionResult> GetAll(
            [FromQuery] int pageNumber = 1,
            [FromQuery] int pageSize = 10)
        {
            var result = await _partRequestService.GetAllAsync(pageNumber, pageSize);
            return Ok(result);
        }

        [HttpGet("search")]
        [Authorize(Roles = "Admin,Staff")]
        public async Task<IActionResult> Search(
            [FromQuery] string? query,
            [FromQuery] PartRequestStatus? status,
            [FromQuery] int pageNumber = 1,
            [FromQuery] int pageSize = 10)
        {
            var result = await _partRequestService.SearchAsync(
                query,
                status,
                pageNumber,
                pageSize);

            return Ok(result);
        }

        [HttpGet("{id:guid}")]
        [Authorize(Roles = "Admin,Staff")]
        public async Task<IActionResult> GetById(Guid id)
        {
            var result = await _partRequestService.GetByIdAsync(id);

            if (!result.Success)
                return NotFound(result);

            return Ok(result);
        }

        [HttpPatch("{id:guid}/sourced")]
        [Authorize(Roles = "Admin,Staff")]
        public async Task<IActionResult> MarkAsSourced(
            Guid id,
            [FromBody] UpdatePartRequestStatusDTO request)
        {
            var result = await _partRequestService.MarkAsSourcedAsync(id, request);

            if (!result.Success)
                return BadRequest(result);

            return Ok(result);
        }

        [HttpPatch("{id:guid}/reject")]
        [Authorize(Roles = "Admin,Staff")]
        public async Task<IActionResult> Reject(
            Guid id,
            [FromBody] UpdatePartRequestStatusDTO request)
        {
            var result = await _partRequestService.RejectAsync(id, request);

            if (!result.Success)
                return BadRequest(result);

            return Ok(result);
        }
    }
}