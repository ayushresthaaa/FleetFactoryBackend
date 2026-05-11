using FleetFactory.Application.Features.SalesInvoices.DTOs;
using FleetFactory.Application.Interfaces.Services;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace FleetFactory.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class SalesInvoiceController(ISalesInvoiceService _salesInvoiceService) : ControllerBase
    {
        [HttpGet]
        public async Task<IActionResult> GetAll(
            [FromQuery] int pageNumber = 1,
            [FromQuery] int pageSize = 10)
        {
            var result = await _salesInvoiceService.GetAllAsync(pageNumber, pageSize);
            return Ok(result);
        }

        [HttpGet("{id:guid}")]
        public async Task<IActionResult> GetById(Guid id)
        {
            var result = await _salesInvoiceService.GetByIdAsync(id);

            if (!result.Success)
                return NotFound(result);

            return Ok(result);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateSalesInvoiceRequestDto request)
        {
            var createdById = User.FindFirstValue(ClaimTypes.NameIdentifier)
                ?? User.FindFirstValue("sub")
                ?? "a486b986-cf38-4280-a1f2-3994477915cb";

            var result = await _salesInvoiceService.CreateAsync(request, createdById);

            if (!result.Success)
                return BadRequest(result);

            return CreatedAtAction(nameof(GetById), new { id = result.Data?.Id }, result);
        }

        [HttpPatch("{id:guid}/mark-paid")]
        public async Task<IActionResult> MarkPaid(Guid id)
        {
            var result = await _salesInvoiceService.MarkPaidAsync(id);

            if (!result.Success)
                return BadRequest(result);

            return Ok(result);
        }

        [HttpPatch("{id:guid}/cancel")]
        public async Task<IActionResult> Cancel(Guid id)
        {
            var result = await _salesInvoiceService.CancelAsync(id);

            if (!result.Success)
                return BadRequest(result);

            return Ok(result);
        }
    }
}