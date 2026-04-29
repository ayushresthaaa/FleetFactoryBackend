using FleetFactory.Application.Interfaces.Services;
using Microsoft.AspNetCore.Mvc;

namespace FleetFactory.API.Controllers
{
    [ApiController]
    [Route("api/customer-side")]
    public class CustomerSideController(ICustomerSideService _customerSideService) : ControllerBase
    {
        [HttpGet("{customerId:guid}/purchase-history")]
        public async Task<IActionResult> GetPurchaseHistory(Guid customerId)
        {
            var result = await _customerSideService
                .GetPurchaseHistoryAsync(customerId);

            if (!result.Success)
                return NotFound(result);

            return Ok(result);
        }
    }
}