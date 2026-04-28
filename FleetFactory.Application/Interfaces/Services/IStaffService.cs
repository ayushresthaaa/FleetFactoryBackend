using FleetFactory.Application.Features.Staff.DTOs;
using FleetFactory.Shared.Results;

namespace FleetFactory.Application.Interfaces.Services
{
    public interface IStaffService
    {
        Task<ApiResponse<StaffResponseDto>> RegisterStaffAsync(RegisterStaffRequestDto request);
        Task<ApiResponse<List<StaffSummaryDto>>> GetAllStaffAsync();
        Task<ApiResponse<StaffResponseDto>> GetStaffByIdAsync(string userId);
        Task<ApiResponse<StaffResponseDto>> UpdateStaffAsync(string userId, UpdateStaffRequestDto request);
        Task<ApiResponse<StaffResponseDto>> SetStaffStatusAsync(string userId, SetStaffStatusRequestDto request);
    }
}