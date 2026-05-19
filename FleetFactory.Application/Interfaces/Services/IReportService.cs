using FleetFactory.Application.Features.Reports.DTOs;
using FleetFactory.Shared.Results;

namespace FleetFactory.Application.Interfaces.Services
{
    public interface IReportService
    {
        Task<ApiResponse<DashboardDTO>> GetAdminDashboardAsync();
        Task<ApiResponse<DashboardDTO>> GetStaffDashboardAsync();

        Task<ApiResponse<FinancialSummaryDTO>> GetFinancialSummaryAsync(DateTime fromDate, DateTime toDate);

        Task<ApiResponse<List<ChartPointDTO>>> GetRevenueTrendAsync(DateTime fromDate, DateTime toDate, string groupBy);

        Task<ApiResponse<List<ReportRowDTO>>> GetTopSellingPartsAsync(DateTime fromDate, DateTime toDate);

        Task<ApiResponse<List<ChartPointDTO>>> GetPaymentMethodsAsync(DateTime fromDate, DateTime toDate);

        Task<ApiResponse<SummaryCardDTO>> GetProfitEstimateAsync(DateTime fromDate, DateTime toDate);

        Task<ApiResponse<List<ReportRowDTO>>> GetHighSpendersAsync(DateTime fromDate, DateTime toDate);

        Task<ApiResponse<List<ReportRowDTO>>> GetRegularCustomersAsync(DateTime fromDate, DateTime toDate);

        Task<ApiResponse<List<PendingCreditDTO>>> GetPendingCreditsAsync();

        Task<ApiResponse<List<ReportRowDTO>>> GetFrequentVehiclesAsync(DateTime fromDate, DateTime toDate);

        Task<ApiResponse<List<ChartPointDTO>>> GetAppointmentStatsAsync(DateTime fromDate, DateTime toDate);
    }
}