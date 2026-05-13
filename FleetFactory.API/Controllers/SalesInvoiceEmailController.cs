using FleetFactory.Application.Interfaces.Services;
using FleetFactory.Domain.Entities;
using Microsoft.AspNetCore.Mvc;

namespace FleetFactory.API.Controllers
{
    [ApiController]
    [Route("api/sales-invoice-email")]
    public class SalesInvoiceEmailController(
        ISalesInvoiceEmailService _salesInvoiceEmailService)
        : ControllerBase
    {
        [HttpPost("send")]
        public async Task<IActionResult> SendInvoiceEmail(
            SendSalesInvoiceMailDto dto)
        {
            var result = await _salesInvoiceEmailService
                .SendSalesInvoiceEmailAsync(dto);

            if (!result)
            {
                return NotFound(new
                {
                    Message = "Customer not found"
                });
            }

            return Ok(new
            {
                Message = "Invoice email sent successfully"
            });
        }
    }
}