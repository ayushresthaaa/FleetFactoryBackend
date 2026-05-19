using FleetFactory.Application.Interfaces.Services;
using Microsoft.AspNetCore.Mvc;

namespace FleetFactory.API.Controllers
{
    [ApiController]
    [Route("api/reports")]
    public class ReportController(
        IReportService _reportService
    ) : ControllerBase
    {
        // Dashboard APIs

        [HttpGet("admin-dashboard")]
        // [Authorize(Roles = "Admin")]
        public async Task<IActionResult> GetAdminDashboard()
        {
            var result = await _reportService.GetAdminDashboardAsync();

            return Ok(result);
        }

        [HttpGet("staff-dashboard")]
        // [Authorize(Roles = "Admin,Staff")]
        public async Task<IActionResult> GetStaffDashboard()
        {
            var result = await _reportService.GetStaffDashboardAsync();

            return Ok(result);
        }

        // Financial reports

        [HttpGet("financial-summary")]
        // [Authorize(Roles = "Admin")]
        public async Task<IActionResult> GetFinancialSummary(
            [FromQuery] DateTime from,
            [FromQuery] DateTime to)
        {
            var result = await _reportService
                .GetFinancialSummaryAsync(from, to);

            if (!result.Success)
                return BadRequest(result);

            return Ok(result);
        }

        [HttpGet("revenue-trend")]
        // [Authorize(Roles = "Admin")]
        public async Task<IActionResult> GetRevenueTrend(
            [FromQuery] DateTime from,
            [FromQuery] DateTime to,
            [FromQuery] string groupBy = "day")
        {
            var result = await _reportService
                .GetRevenueTrendAsync(from, to, groupBy);

            if (!result.Success)
                return BadRequest(result);

            return Ok(result);
        }

        [HttpGet("top-selling-parts")]
        // [Authorize(Roles = "Admin,Staff")]
        public async Task<IActionResult> GetTopSellingParts(
            [FromQuery] DateTime from,
            [FromQuery] DateTime to)
        {
            var result = await _reportService
                .GetTopSellingPartsAsync(from, to);

            if (!result.Success)
                return BadRequest(result);

            return Ok(result);
        }

        [HttpGet("payment-methods")]
        // [Authorize(Roles = "Admin")]
        public async Task<IActionResult> GetPaymentMethods(
            [FromQuery] DateTime from,
            [FromQuery] DateTime to)
        {
            var result = await _reportService
                .GetPaymentMethodsAsync(from, to);

            if (!result.Success)
                return BadRequest(result);

            return Ok(result);
        }

        [HttpGet("profit-estimate")]
        // [Authorize(Roles = "Admin")]
        public async Task<IActionResult> GetProfitEstimate(
            [FromQuery] DateTime from,
            [FromQuery] DateTime to)
        {
            var result = await _reportService
                .GetProfitEstimateAsync(from, to);

            if (!result.Success)
                return BadRequest(result);

            return Ok(result);
        }

        // Customer / staff reports

        [HttpGet("high-spenders")]
        // [Authorize(Roles = "Admin,Staff")]
        public async Task<IActionResult> GetHighSpenders(
            [FromQuery] DateTime from,
            [FromQuery] DateTime to)
        {
            var result = await _reportService
                .GetHighSpendersAsync(from, to);

            if (!result.Success)
                return BadRequest(result);

            return Ok(result);
        }

        [HttpGet("regular-customers")]
        // [Authorize(Roles = "Admin,Staff")]
        public async Task<IActionResult> GetRegularCustomers(
            [FromQuery] DateTime from,
            [FromQuery] DateTime to)
        {
            var result = await _reportService
                .GetRegularCustomersAsync(from, to);

            if (!result.Success)
                return BadRequest(result);

            return Ok(result);
        }

        [HttpGet("pending-credits")]
        // [Authorize(Roles = "Admin,Staff")]
        public async Task<IActionResult> GetPendingCredits()
        {
            var result = await _reportService
                .GetPendingCreditsAsync();

            return Ok(result);
        }

        [HttpGet("frequent-vehicles")]
        // [Authorize(Roles = "Admin,Staff")]
        public async Task<IActionResult> GetFrequentVehicles(
            [FromQuery] DateTime from,
            [FromQuery] DateTime to)
        {
            var result = await _reportService
                .GetFrequentVehiclesAsync(from, to);

            if (!result.Success)
                return BadRequest(result);

            return Ok(result);
        }

        [HttpGet("appointment-stats")]
        // [Authorize(Roles = "Admin,Staff")]
        public async Task<IActionResult> GetAppointmentStats(
            [FromQuery] DateTime from,
            [FromQuery] DateTime to)
        {
            var result = await _reportService
                .GetAppointmentStatsAsync(from, to);

            if (!result.Success)
                return BadRequest(result);

            return Ok(result);
        }
    }
}