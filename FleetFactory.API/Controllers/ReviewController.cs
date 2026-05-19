using System.Security.Claims;
using FleetFactory.Application.Features.Reviews.DTOs;
using FleetFactory.Application.Interfaces.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FleetFactory.API.Controllers
{
    [ApiController]
    [Route("api/reviews")]
    public class ReviewsController(IReviewService _reviewService) : ControllerBase
    {
        [HttpPost("me")]
        // [Authorize(Roles = "Customer")]
        public async Task<IActionResult> CreateMyReview(
            [FromBody] CreateReviewRequestDTO request)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier)
                ?? User.FindFirstValue("sub");

            if (string.IsNullOrWhiteSpace(userId))
                return Unauthorized();

            var result = await _reviewService.CreateMyReviewAsync(userId, request);

            if (!result.Success)
                return BadRequest(result);

            return Ok(result);
        }

        [HttpGet("me")]
        // [Authorize(Roles = "Customer")]
        public async Task<IActionResult> GetMyReviews()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier)
                ?? User.FindFirstValue("sub");

            if (string.IsNullOrWhiteSpace(userId))
                return Unauthorized();

            var result = await _reviewService.GetMyReviewsAsync(userId);

            if (!result.Success)
                return NotFound(result);

            return Ok(result);
        }

        [HttpGet("me/{id:guid}")]
        // [Authorize(Roles = "Customer")]
        public async Task<IActionResult> GetMyReviewById(Guid id)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier)
                ?? User.FindFirstValue("sub");

            if (string.IsNullOrWhiteSpace(userId))
                return Unauthorized();

            var result = await _reviewService.GetMyReviewByIdAsync(userId, id);

            if (!result.Success)
                return NotFound(result);

            return Ok(result);
        }

        [HttpGet]
        // [Authorize(Roles = "Admin,Staff")]
        public async Task<IActionResult> GetAll(
            [FromQuery] int pageNumber = 1,
            [FromQuery] int pageSize = 10)
        {
            var result = await _reviewService.GetAllAsync(pageNumber, pageSize);
            return Ok(result);
        }

        [HttpGet("{id:guid}")]
        // [Authorize(Roles = "Admin,Staff")]
        public async Task<IActionResult> GetById(Guid id)
        {
            var result = await _reviewService.GetByIdAsync(id);

            if (!result.Success)
                return NotFound(result);

            return Ok(result);
        }

        [HttpGet("customer/{customerId:guid}")]
        // [Authorize(Roles = "Admin,Staff")]
        public async Task<IActionResult> GetByCustomerId(Guid customerId)
        {
            var result = await _reviewService.GetByCustomerIdAsync(customerId);
            return Ok(result);
        }

        [HttpPatch("{id:guid}/hide")]
        // [Authorize(Roles = "Admin,Staff")]
        public async Task<IActionResult> Hide(Guid id)
        {
            var result = await _reviewService.HideAsync(id);

            if (!result.Success)
                return BadRequest(result);

            return Ok(result);
        }

        [HttpPatch("{id:guid}/show")]
        // [Authorize(Roles = "Admin,Staff")]
        public async Task<IActionResult> Show(Guid id)
        {
            var result = await _reviewService.ShowAsync(id);

            if (!result.Success)
                return BadRequest(result);

            return Ok(result);
        }
    }
}