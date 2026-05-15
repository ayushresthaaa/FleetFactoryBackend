using FleetFactory.Application.Features.Reports.DTOs;
using FleetFactory.Shared.Results;
using FleetFactory.Domain.Entities;
namespace FleetFactory.Application.Interfaces.Services
{
    public interface IReportService
    {
        Task<ApiResponse<FinancialReportResponseDTO>> GetFinancialReportAsync(
            string type,
            DateTime? fromDate = null,
            DateTime? toDate = null);
        Task<List<CustomerProfile>> GetUnpaidCreditReportAsync();
        Task<List<CustomerProfile>> GetCustomersWithCreditAsync();

        Task<List<CustomerReportResponseDTO>> GetRegularCustomersReportAsync(
            DateTime? fromDate = null,
            DateTime? toDate = null);

        Task<List<CustomerReportResponseDTO>> GetHighSpendersReportAsync(
            DateTime? fromDate = null,
            DateTime? toDate = null);

        Task<List<CustomerReportResponseDTO>> GetPendingCreditCustomersReportAsync(
            DateTime? fromDate = null,
            DateTime? toDate = null);
    }
}