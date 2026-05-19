using FleetFactory.Application.Features.OverdueCredits.DTOs;
using FleetFactory.Shared.Results;

namespace FleetFactory.Application.Interfaces.Services
{
    public interface IOverdueCreditService
    {
        Task<ApiResponse<List<OverdueCreditCustomerDTO>>> GetOverdueCreditsAsync();

        Task<ApiResponse<string>> SendReminderToCustomerAsync(Guid customerId);

        Task<ApiResponse<int>> SendReminderToAllAsync();
    }
}