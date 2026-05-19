using FleetFactory.Application.Features.Reports.DTOs;

namespace FleetFactory.Application.Interfaces.Repositories
{
    public interface IReportRepository
    {
        // Dashboard
        Task<DashboardDTO> GetAdminDashboardAsync();
        Task<DashboardDTO> GetStaffDashboardAsync();

        // Financial reports
        Task<FinancialSummaryDTO> GetFinancialSummaryAsync(DateTime fromDate, DateTime toDate);

        Task<List<ChartPointDTO>> GetRevenueTrendAsync(
            DateTime fromDate,
            DateTime toDate,
            string groupBy
        );

        Task<List<ReportRowDTO>> GetTopSellingPartsAsync(
            DateTime fromDate,
            DateTime toDate
        );

        Task<List<ChartPointDTO>> GetPaymentMethodsAsync(
            DateTime fromDate,
            DateTime toDate
        );

        Task<SummaryCardDTO> GetProfitEstimateAsync(
            DateTime fromDate,
            DateTime toDate
        );

        // Customer / staff reports
        Task<List<ReportRowDTO>> GetHighSpendersAsync(
            DateTime fromDate,
            DateTime toDate
        );

        Task<List<ReportRowDTO>> GetRegularCustomersAsync(
            DateTime fromDate,
            DateTime toDate
        );

        Task<List<PendingCreditDTO>> GetPendingCreditsAsync();

        Task<List<ReportRowDTO>> GetFrequentVehiclesAsync(
            DateTime fromDate,
            DateTime toDate
        );

        Task<List<ChartPointDTO>> GetAppointmentStatsAsync(
            DateTime fromDate,
            DateTime toDate
        );
    }
}