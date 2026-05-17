using FleetFactory.Application.Features.SalesInvoices.DTOs;
using FleetFactory.Application.Interfaces.Services;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using FleetFactory.Domain.Enums;
namespace FleetFactory.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class SalesInvoiceController(ISalesInvoiceService _salesInvoiceService, ISendInvoiceEmailService _sendInvoiceEmailService) : ControllerBase
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
                ?? User.FindFirstValue("sub");

            if (string.IsNullOrWhiteSpace(createdById))
                return Unauthorized();

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
        //works with the search method in the repository and service to allow searching by customer name, invoice number, and filtering by status. Pagination is also supported.
        [HttpGet("search")]
        public async Task<IActionResult> Search(
            [FromQuery] string? query,
            [FromQuery] InvoiceStatus? status,
            [FromQuery] SalesInvoiceMode? mode, //used to seggregate between parts and appointment invoices; for frontend easiness
            [FromQuery] int pageNumber = 1,
            [FromQuery] int pageSize = 10)
        {
            var result = await _salesInvoiceService.SearchAsync(
                query,
                status,
                mode,
                pageNumber,
                pageSize);

            return Ok(result);
        }

        [HttpGet("customer/{customerId:guid}/appointments")]
        public async Task<IActionResult> GetCustomerAppointments(Guid customerId)
        {
            var result = await _salesInvoiceService.GetCustomerAppointmentsAsync(customerId);

            if (!result.Success)
                return NotFound(result);

            return Ok(result);
        }

        //rabison : to send sales invoice email to customer
        [HttpPost("{id:guid}/send-email")]
            public async Task<IActionResult> SendInvoiceEmail(Guid id)
        {
            var result = await _sendInvoiceEmailService
                .SendSalesInvoiceEmailAsync(id);

            if (!result.Success)
                return BadRequest(result);

            return Ok(result);
        }
    }
}