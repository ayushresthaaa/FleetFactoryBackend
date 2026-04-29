using FleetFactory.Application.Features.CustomerSide.DTOs;
using FleetFactory.Shared.Results;

namespace FleetFactory.Application.Interfaces.Services
{
    public interface ICustomerSideService
    {
        Task<ApiResponse<List<CustomerPurchaseHistoryResponseDto>>> 
            GetPurchaseHistoryAsync(Guid customerId);
    }
}