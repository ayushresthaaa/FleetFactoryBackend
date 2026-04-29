using FleetFactory.Application.Features.Reports.DTOs;
using FleetFactory.Shared.Results;

namespace FleetFactory.Application.Interfaces.Services
{
    public interface IReportService
    {
        Task<ApiResponse<FinancialReportResponseDTO>> GetFinancialReportAsync(string type);
    }
}