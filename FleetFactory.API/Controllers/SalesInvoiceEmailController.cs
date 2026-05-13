using FleetFactory.Application.Interfaces.Services;
using FleetFactory.Domain.Entities;
using Microsoft.AspNetCore.Mvc;

namespace FleetFactory.API.Controllers
{
    [ApiController]
    [Route("api/sales-invoice-email")]
     private readonly ISalesInvoiceEmailService _service;

    public SalesInvoiceEmailController(
        ISalesInvoiceEmailService service)
    {
        _service = service;
    }

    [HttpPost("send")]
    public async Task<IActionResult> SendInvoiceMail(
        SendSalesInvoiceMailRequestDTO request)
    {
        var result = await _service
            .SendSalesInvoiceMailAsync(request.SalesInvoiceId);

        if (!result)
        {
            return BadRequest("Invoice not found");
        }

        return Ok("Invoice mail sent successfully");
    }
}