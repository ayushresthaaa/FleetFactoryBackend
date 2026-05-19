using FleetFactory.Application.Interfaces.Services;
using Microsoft.AspNetCore.Mvc;

namespace FleetFactory.API.Controllers
{
    [ApiController]
    [Route("api/low-stock")]
    public class LowStockController(ILowStockService _lowStockService) : ControllerBase
    {
      [HttpPost("check")]
        public async Task<IActionResult> CheckLowStock()
        {
            var result = await _lowStockService.CheckLowStockAsync();
            return Ok(result);
        }
    }
}