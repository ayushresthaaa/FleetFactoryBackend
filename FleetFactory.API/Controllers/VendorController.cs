using FleetFactory.Application.Features.Vendors.DTOs;
using FleetFactory.Application.Interfaces.Services;
using Microsoft.AspNetCore.Mvc;

namespace FleetFactory.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class VendorsController(IVendorService _vendorService) : ControllerBase
    {
        [HttpGet]
        public async Task<IActionResult> GetAll(
            [FromQuery] int pageNumber = 1,
            [FromQuery] int pageSize = 10)
        {
            var result = await _vendorService.GetAllAsync(pageNumber, pageSize);
            return Ok(result);
        }

        [HttpGet("{id:guid}")]
        public async Task<IActionResult> GetById(Guid id)
        {
            var result = await _vendorService.GetByIdAsync(id);

            if (!result.Success)
                return NotFound(result);

            return Ok(result);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateVendorRequestDto request)
        {
            var result = await _vendorService.CreateAsync(request);

            if (!result.Success)
                return BadRequest(result);

            return CreatedAtAction(nameof(GetById), new { id = result.Data?.Id }, result);
        }

        [HttpPut("{id:guid}")]
        public async Task<IActionResult> Update(Guid id, [FromBody] UpdateVendorRequestDto request)
        {
            var result = await _vendorService.UpdateAsync(id, request);

            if (!result.Success)
                return BadRequest(result);

            return Ok(result);
        }

        [HttpDelete("{id:guid}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            var result = await _vendorService.DeleteAsync(id);

            if (!result.Success)
                return NotFound(result);

            return Ok(result);
        }
    }
}