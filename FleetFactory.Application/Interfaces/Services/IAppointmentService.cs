using FleetFactory.Application.Features.Appointments.DTOs;
using FleetFactory.Shared.Results;

namespace FleetFactory.Application.Interfaces.Services
{
    public interface IAppointmentService
    {
        Task<ApiResponse<PagedResult<AppointmentResponseDTO>>> GetAllAsync(
            int pageNumber,
            int pageSize);

        Task<ApiResponse<AppointmentResponseDTO>> GetByIdAsync(Guid id);

        Task<ApiResponse<AppointmentResponseDTO>> CreateMyAppointmentAsync(
            string userId,
            CreateMyAppointmentRequestDTO request);

        Task<ApiResponse<AppointmentResponseDTO>> ConfirmAsync(Guid id);

        Task<ApiResponse<AppointmentResponseDTO>> CancelAsync(Guid id);
    }
}