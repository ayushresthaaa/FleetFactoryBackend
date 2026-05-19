using FleetFactory.Application.Interfaces.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;

namespace FleetFactory.API.Controllers
{
    [ApiController]
    [Route("api/low-stock")]
    [Authorize(Roles = "Admin,Staff")]
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