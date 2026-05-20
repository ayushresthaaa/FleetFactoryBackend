using FleetFactory.Application.Features.Reports.DTOs;
using FleetFactory.Application.Interfaces.Repositories;
using FleetFactory.Application.Interfaces.Services;
using FleetFactory.Shared.Results;

namespace FleetFactory.Application.Features.Reports.Services
{
    public class ReportService(IReportRepository _reportRepository) : IReportService
    {
        public async Task<ApiResponse<DashboardDTO>> GetAdminDashboardAsync()
        {
            var result = await _reportRepository.GetAdminDashboardAsync();

            return ApiResponse<DashboardDTO>
                .SuccessResponse(result, "Admin dashboard loaded successfully");
        }

        public async Task<ApiResponse<DashboardDTO>> GetStaffDashboardAsync()
        {
            var result = await _reportRepository.GetStaffDashboardAsync();

            return ApiResponse<DashboardDTO>
                .SuccessResponse(result, "Staff dashboard loaded successfully");
        }

        public async Task<ApiResponse<FinancialSummaryDTO>> GetFinancialSummaryAsync(
            DateTime fromDate,
            DateTime toDate
        )
        {
            var validation = ValidateDateRange(fromDate, toDate);
            if (validation != null) return validation;

            fromDate = ToUtcDate(fromDate);
            toDate = ToUtcEndDate(toDate);

            var result = await _reportRepository.GetFinancialSummaryAsync(fromDate, toDate);

            return ApiResponse<FinancialSummaryDTO>
                .SuccessResponse(result, "Financial summary generated successfully");
        }

        public async Task<ApiResponse<List<ChartPointDTO>>> GetRevenueTrendAsync(
            DateTime fromDate,
            DateTime toDate,
            string groupBy
        )
        {
            var validation = ValidateDateRangeForList<ChartPointDTO>(fromDate, toDate);
            if (validation != null) return validation;

            groupBy = groupBy.ToLower();

            if (groupBy != "day" && groupBy != "month")
            {
                return ApiResponse<List<ChartPointDTO>>
                    .ErrorResponse("Invalid groupBy. Use 'day' or 'month'.");
            }

            fromDate = ToUtcDate(fromDate);
            toDate = ToUtcEndDate(toDate);

            var result = await _reportRepository.GetRevenueTrendAsync(fromDate, toDate, groupBy);

            return ApiResponse<List<ChartPointDTO>>
                .SuccessResponse(result, "Revenue trend generated successfully");
        }

        public async Task<ApiResponse<List<ReportRowDTO>>> GetTopSellingPartsAsync(
            DateTime fromDate,
            DateTime toDate
        )
        {
            var validation = ValidateDateRangeForList<ReportRowDTO>(fromDate, toDate);
            if (validation != null) return validation;

            fromDate = ToUtcDate(fromDate);
            toDate = ToUtcEndDate(toDate);

            var result = await _reportRepository.GetTopSellingPartsAsync(fromDate, toDate);

            return ApiResponse<List<ReportRowDTO>>
                .SuccessResponse(result, "Top selling parts report generated successfully");
        }

        public async Task<ApiResponse<List<ChartPointDTO>>> GetPaymentMethodsAsync(
            DateTime fromDate,
            DateTime toDate
        )
        {
            var validation = ValidateDateRangeForList<ChartPointDTO>(fromDate, toDate);
            if (validation != null) return validation;

            fromDate = ToUtcDate(fromDate);
            toDate = ToUtcEndDate(toDate);

            var result = await _reportRepository.GetPaymentMethodsAsync(fromDate, toDate);

            return ApiResponse<List<ChartPointDTO>>
                .SuccessResponse(result, "Payment methods report generated successfully");
        }

        public async Task<ApiResponse<SummaryCardDTO>> GetProfitEstimateAsync(
            DateTime fromDate,
            DateTime toDate
        )
        {
            var validation = ValidateDateRangeForSummaryCard(fromDate, toDate);
            if (validation != null) return validation;

            fromDate = ToUtcDate(fromDate);
            toDate = ToUtcEndDate(toDate);

            var result = await _reportRepository.GetProfitEstimateAsync(fromDate, toDate);

            return ApiResponse<SummaryCardDTO>
                .SuccessResponse(result, "Profit estimate generated successfully");
        }

        public async Task<ApiResponse<List<ReportRowDTO>>> GetHighSpendersAsync(
            DateTime fromDate,
            DateTime toDate
        )
        {
            var validation = ValidateDateRangeForList<ReportRowDTO>(fromDate, toDate);
            if (validation != null) return validation;

            fromDate = ToUtcDate(fromDate);
            toDate = ToUtcEndDate(toDate);

            var result = await _reportRepository.GetHighSpendersAsync(fromDate, toDate);

            return ApiResponse<List<ReportRowDTO>>
                .SuccessResponse(result, "High spenders report generated successfully");
        }

        public async Task<ApiResponse<List<ReportRowDTO>>> GetRegularCustomersAsync(
            DateTime fromDate,
            DateTime toDate
        )
        {
            var validation = ValidateDateRangeForList<ReportRowDTO>(fromDate, toDate);
            if (validation != null) return validation;

            fromDate = ToUtcDate(fromDate);
            toDate = ToUtcEndDate(toDate);

            var result = await _reportRepository.GetRegularCustomersAsync(fromDate, toDate);

            return ApiResponse<List<ReportRowDTO>>
                .SuccessResponse(result, "Regular customers report generated successfully");
        }

        public async Task<ApiResponse<List<PendingCreditDTO>>> GetPendingCreditsAsync()
        {
            var result = await _reportRepository.GetPendingCreditsAsync();

            return ApiResponse<List<PendingCreditDTO>>
                .SuccessResponse(result, "Pending credits report generated successfully");
        }

        public async Task<ApiResponse<List<ReportRowDTO>>> GetFrequentVehiclesAsync(
            DateTime fromDate,
            DateTime toDate
        )
        {
            var validation = ValidateDateRangeForList<ReportRowDTO>(fromDate, toDate);
            if (validation != null) return validation;

            fromDate = ToUtcDate(fromDate);
            toDate = ToUtcEndDate(toDate);

            var result = await _reportRepository.GetFrequentVehiclesAsync(fromDate, toDate);

            return ApiResponse<List<ReportRowDTO>>
                .SuccessResponse(result, "Frequent vehicles report generated successfully");
        }

        public async Task<ApiResponse<List<ChartPointDTO>>> GetAppointmentStatsAsync(
            DateTime fromDate,
            DateTime toDate
        )
        {
            var validation = ValidateDateRangeForList<ChartPointDTO>(fromDate, toDate);
            if (validation != null) return validation;

            fromDate = ToUtcDate(fromDate);
            toDate = ToUtcEndDate(toDate);

            var result = await _reportRepository.GetAppointmentStatsAsync(fromDate, toDate);

            return ApiResponse<List<ChartPointDTO>>
                .SuccessResponse(result, "Appointment stats report generated successfully");
        }

        private static DateTime ToUtcDate(DateTime date)
        {
            return DateTime.SpecifyKind(date.Date, DateTimeKind.Utc);
        }

        private static DateTime ToUtcEndDate(DateTime date)
        {
            return DateTime.SpecifyKind(
                date.Date.AddDays(1).AddTicks(-1),
                DateTimeKind.Utc
            );
        }

        private ApiResponse<FinancialSummaryDTO>? ValidateDateRange(DateTime fromDate, DateTime toDate)
        {
            if (fromDate > toDate)
            {
                return ApiResponse<FinancialSummaryDTO>
                    .ErrorResponse("From date cannot be greater than to date.");
            }

            return null;
        }

        private ApiResponse<List<T>>? ValidateDateRangeForList<T>(DateTime fromDate, DateTime toDate)
        {
            if (fromDate > toDate)
            {
                return ApiResponse<List<T>>
                    .ErrorResponse("From date cannot be greater than to date.");
            }

            return null;
        }

        private ApiResponse<SummaryCardDTO>? ValidateDateRangeForSummaryCard(DateTime fromDate, DateTime toDate)
        {
            if (fromDate > toDate)
            {
                return ApiResponse<SummaryCardDTO>
                    .ErrorResponse("From date cannot be greater than to date.");
            }

            return null;
        }
    }
}