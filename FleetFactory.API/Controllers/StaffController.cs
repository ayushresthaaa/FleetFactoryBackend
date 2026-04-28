using FleetFactory.Application.Features.Staff.DTOs;
using FleetFactory.Application.Interfaces.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FleetFactory.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize(Roles = "Admin")]
    public class StaffController(IStaffService _staffService) : ControllerBase
    {
        [HttpPost]
        public async Task<IActionResult> Register([FromBody] RegisterStaffRequestDto request)
        {
            var result = await _staffService.RegisterStaffAsync(request);

            if (!result.Success)
                return BadRequest(result);

            return Created("", result);
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var result = await _staffService.GetAllStaffAsync();
            return Ok(result);
        }

        [HttpGet("{userId}")]
        public async Task<IActionResult> GetById(string userId)
        {
            var result = await _staffService.GetStaffByIdAsync(userId);

            if (!result.Success)
                return NotFound(result);

            return Ok(result);
        }

        [HttpPut("{userId}")]
        public async Task<IActionResult> Update(string userId, [FromBody] UpdateStaffRequestDto request)
        {
            var result = await _staffService.UpdateStaffAsync(userId, request);

            if (!result.Success)
                return BadRequest(result);

            return Ok(result);
        }

        //PATCH for status toggle 
        [HttpPatch("{userId}/status")]
        public async Task<IActionResult> SetStatus(string userId, [FromBody] SetStaffStatusRequestDto request)
        {
            var result = await _staffService.SetStaffStatusAsync(userId, request);

            if (!result.Success)
                return NotFound(result);

            return Ok(result);
        }
    }
}
