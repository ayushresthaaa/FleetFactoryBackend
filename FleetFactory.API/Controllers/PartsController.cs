using FleetFactory.Application.Features.Parts.DTOs;
using FleetFactory.Application.Interfaces.Services;
using Microsoft.AspNetCore.Mvc;

namespace FleetFactory.API.Controllers
{

    [ApiController]
    [Route("api/[controller]")]
    public class PartsController (IPartService _partService) : ControllerBase
    {
        [HttpGet]
        public async Task<IActionResult> GetAll(
            [FromQuery] int pageNumber = 1,
            [FromQuery] int pageSize = 10)
        {
            var result = await _partService.GetAllAsync(pageNumber, pageSize);
            return Ok(result);
        }

        [HttpGet("{id:guid}")]
        public async Task<IActionResult> GetById(Guid id)
        {
            var result = await _partService.GetByIdAsync(id);

            if (!result.Success)
                return NotFound(result);

            return Ok(result);
        }

       [HttpPost]
        public async Task<IActionResult> Create([FromForm] CreatePartRequestDto request)
        {
            var result = await _partService.CreateAsync(request);

            if (!result.Success)
                return BadRequest(result);

            return CreatedAtAction(nameof(GetById), new { id = result.Data?.Id }, result);
        }

       [HttpPut("{id:guid}")]
        public async Task<IActionResult> Update(Guid id, [FromForm] UpdatePartRequestDto request)
        {
            var result = await _partService.UpdateAsync(id, request);

            if (!result.Success)
                return BadRequest(result);

            return Ok(result);
        }

        [HttpDelete("{id:guid}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            var result = await _partService.DeleteAsync(id);

            if (!result.Success)
                return NotFound(result);

            return Ok(result);
        }

        [HttpGet("search")]
        public async Task<IActionResult> Search(
            [FromQuery] string keyword,
            [FromQuery] int pageNumber = 1,
            [FromQuery] int pageSize = 10)
        {
            var result = await _partService.SearchAsync(
                keyword,
                pageNumber,
                pageSize);

            return Ok(result);
        }

        [HttpGet("low-stock")]
        public async Task<IActionResult> GetLowStock(
            [FromQuery] int threshold = 10)
        {
            var result = await _partService.GetLowStockAsync(threshold);

            return Ok(result);
        }

        [HttpGet("available")]
        public async Task<IActionResult> GetAvailable()
        {
            var result = await _partService.GetAvailableAsync();

            return Ok(result);
        }

        [HttpGet("{id:guid}/stock-movements")]
        public async Task<IActionResult> GetStockMovements(Guid id)
        {
            var result = await _partService.GetStockMovementsAsync(id);

            if (!result.Success)
                return NotFound(result);

            return Ok(result);
        }

        [HttpGet("vendor/{vendorId:guid}")]
        public async Task<IActionResult> GetByVendor(
            Guid vendorId,
            [FromQuery] int pageNumber = 1,
            [FromQuery] int pageSize = 10)
        {
            var result = await _partService.GetByVendorAsync(
                vendorId,
                pageNumber,
                pageSize);

            return Ok(result);
        }

        [HttpGet("category/{categoryId:guid}")]
        public async Task<IActionResult> GetByCategory(
            Guid categoryId,
            [FromQuery] int pageNumber = 1,
            [FromQuery] int pageSize = 10)
        {
            var result = await _partService.GetByCategoryAsync(
                categoryId,
                pageNumber,
                pageSize);

            return Ok(result);
        }
    }
}