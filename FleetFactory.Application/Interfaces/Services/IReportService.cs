using FleetFactory.Application.Features.Reports.DTOs;
using FleetFactory.Shared.Results;
using FleetFactory.Domain.Entities;
namespace FleetFactory.Application.Interfaces.Services
{
    public interface IReportService
    {
        Task<ApiResponse<FinancialReportResponseDTO>> GetFinancialReportAsync(string type);
        Task<List<CustomerProfile>> GetUnpaidCreditReportAsync();
        Task<List<CustomerProfile>> GetCustomersWithCreditAsync();
    }
}