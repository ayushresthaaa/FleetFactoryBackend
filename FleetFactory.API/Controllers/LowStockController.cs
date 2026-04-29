using FleetFactory.Application.Interfaces.Services;
using Microsoft.AspNetCore.Mvc;

namespace FleetFactory.API.Controllers
{
    [ApiController]
    [Route("api/low-stock")]
    public class LowStockController(ILowStockService _lowStockService) : ControllerBase
    {
        [HttpPost("check")]
        public async Task<IActionResult> CheckLowStock([FromQuery] int threshold = 10)
        {
            var result = await _lowStockService.CheckLowStockAsync(threshold);
            return Ok(result);
        }
    }
}