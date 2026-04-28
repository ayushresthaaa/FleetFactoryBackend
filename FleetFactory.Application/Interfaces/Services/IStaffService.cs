using FleetFactory.Application.Features.Staff.DTOs;
using FleetFactory.Shared.Results;

namespace FleetFactory.Application.Interfaces.Services
{
    public interface IStaffService
    {
        Task<ApiResponse<PagedResult<StaffResponseDTO>>> GetAllAsync(int pageNumber, int pageSize);

        Task<ApiResponse<StaffResponseDTO>> GetByIdAsync(Guid id);

        Task<ApiResponse<StaffResponseDTO>> CreateAsync(CreateStaffRequestDTO request);

        Task<ApiResponse<StaffResponseDTO>> UpdateAsync(Guid id, UpdateStaffRequestDTO request);

        Task<ApiResponse<string>> DeactivateAsync(Guid id);
    }
}