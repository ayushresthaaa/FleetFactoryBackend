using FleetFactory.Application.Features.Customers.DTOs;
using FleetFactory.Application.Interfaces.Services;
using Microsoft.AspNetCore.Mvc;

namespace FleetFactory.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CustomersController(ICustomerService _customerService) : ControllerBase
    {
        [HttpGet]
        public async Task<IActionResult> GetAll(
            [FromQuery] int pageNumber = 1,
            [FromQuery] int pageSize = 10)
        {
            var result = await _customerService.GetAllAsync(pageNumber, pageSize);
            return Ok(result);
        }

        [HttpGet("{id:guid}")]
        public async Task<IActionResult> GetById(Guid id)
        {
            var result = await _customerService.GetByIdAsync(id);

            if (!result.Success)
                return NotFound(result);

            return Ok(result);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateCustomerWithVehicleRequestDto request)
        {
            var result = await _customerService.CreateAsync(request);

            if (!result.Success)
                return BadRequest(result);

            return CreatedAtAction(nameof(GetById), new { id = result.Data?.Id }, result);
        }
        

        [HttpPost("{customerId:guid}/vehicles")]
        public async Task<IActionResult> AddVehicle(
            Guid customerId,
            [FromBody] AddVehicleRequestDto request)
        {
            var result = await _customerService.AddVehicleAsync(customerId, request);

            if (!result.Success)
                return BadRequest(result);

            return Ok(result);
        }
    }
}