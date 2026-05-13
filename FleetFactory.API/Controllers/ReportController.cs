using FleetFactory.Application.Interfaces.Services;
using Microsoft.AspNetCore.Mvc;

namespace FleetFactory.API.Controllers
{
    [ApiController]
    [Route("api/reports")]
    public class ReportsController(IReportService _reportService) : ControllerBase
    {
        [HttpGet("financial")]
        public async Task<IActionResult> GetFinancialReport([FromQuery] string type = "daily")
        {
            var result = await _reportService.GetFinancialReportAsync(type);

            if (!result.Success)
                return BadRequest(result);

            return Ok(result);
        }

         // Customers with overdue / unpaid credit (30+ days logic in service)
        [HttpGet("credit/unpaid")]
        public async Task<IActionResult> GetUnpaidCreditReport()
        {
            var result = await _reportService.GetUnpaidCreditReportAsync();
            return Ok(result);
        }

        // All customers currently using credit
        [HttpGet("credit/all")]
        public async Task<IActionResult> GetCustomersWithCredit()
        {
            var result = await _reportService.GetCustomersWithCreditAsync();
            return Ok(result);
        }
    }
}