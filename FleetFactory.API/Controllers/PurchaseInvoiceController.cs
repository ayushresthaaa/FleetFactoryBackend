using FleetFactory.Application.Features.PurchaseInvoices.DTOs;
using FleetFactory.Application.Interfaces.Services;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
namespace FleetFactory.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PurchaseInvoiceController(IPurchaseInvoiceService _purchaseInvoiceService) : ControllerBase
    {
        [HttpGet]
        public async Task<IActionResult> GetAll(
            [FromQuery] int pageNumber = 1,
            [FromQuery] int pageSize = 10)
        {
            var result = await _purchaseInvoiceService.GetAllAsync(pageNumber, pageSize);
            return Ok(result);
        }

        [HttpGet("{id:guid}")]
        public async Task<IActionResult> GetById(Guid id)
        {
            var result = await _purchaseInvoiceService.GetByIdAsync(id);

            if (!result.Success)
                return NotFound(result);

            return Ok(result);
        }

        // need to add token here for the createdById to track who created the invoice, but for now we will just pass a dummy user id
         [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreatePurchaseInvoiceRequestDto request)
        {
            // temporary for now until auth is added properly
           var createdById = User.FindFirstValue(ClaimTypes.NameIdentifier)
                ?? User.FindFirstValue("sub")
                ?? "a486b986-cf38-4280-a1f2-3994477915cb"; 

            var result = await _purchaseInvoiceService.CreateAsync(request, createdById);

            if (!result.Success)
                return BadRequest(result);

            return CreatedAtAction(nameof(GetById), new { id = result.Data?.Id }, result);
        }

        [HttpPatch("{id:guid}/status")]
        public async Task<IActionResult> StatusUpdate(Guid id)
        {
            var result = await _purchaseInvoiceService.StatusUpdateAsync(id);

            if (!result.Success)
                return NotFound(result);

            return Ok(result);
        }

        [HttpPatch("{id:guid}/receive")]
        public async Task<IActionResult> Receive(Guid id)
        {
            var receivedById = User.FindFirstValue(ClaimTypes.NameIdentifier)
                ?? User.FindFirstValue("sub")
                ?? "a486b986-cf38-4280-a1f2-3994477915cb";

            var result = await _purchaseInvoiceService.ReceiveAsync(id, receivedById);

            if (!result.Success)
                return BadRequest(result);

            return Ok(result);
        }

        [HttpPatch("{id:guid}/cancel")]
        public async Task<IActionResult> Cancel(Guid id)
        {
            var result = await _purchaseInvoiceService.CancelAsync(id);

            if (!result.Success)
                return BadRequest(result);

            return Ok(result);
        }
    }
}