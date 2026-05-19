using FleetFactory.Application.Interfaces.Services;
using FleetFactory.Domain.Enums;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
namespace FleetFactory.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize(Roles = "Admin,Staff")]
    public class CustomerLookupController(
        ICustomerLookupService _customerLookupService
    ) : ControllerBase
    {
        [HttpGet]
        public async Task<IActionResult> Search(
            [FromQuery] string query,
            [FromQuery] CustomerLookupType type = CustomerLookupType.All)
        {
            var result = await _customerLookupService.SearchAsync(query, type);

            if (!result.Success)
                return BadRequest(result);

            return Ok(result);
        }
    }
}