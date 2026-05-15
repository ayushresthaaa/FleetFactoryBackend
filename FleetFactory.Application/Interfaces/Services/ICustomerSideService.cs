using FleetFactory.Application.Features.CustomerSide.DTOs;
using FleetFactory.Shared.Results;

namespace FleetFactory.Application.Interfaces.Services
{
    public interface ICustomerSideService
    {
        Task<ApiResponse<List<CustomerPurchaseHistoryResponseDto>>>
            GetMyPurchaseHistoryAsync(string userId);

        Task<ApiResponse<List<CustomerAppointmentHistoryResponseDto>>>
            GetMyAppointmentHistoryAsync(string userId);

        Task<ApiResponse<List<CustomerAppointmentHistoryResponseDto>>>
            GetMyUpcomingAppointmentsAsync(string userId);

        Task<ApiResponse<CustomerAppointmentHistoryResponseDto>>
            GetMyAppointmentByIdAsync(string userId, Guid appointmentId);
    }
}