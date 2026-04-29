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
    }
}