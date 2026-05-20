using FleetFactory.Application.Features.Account.DTOs;
using FleetFactory.Shared.Results;

namespace FleetFactory.Application.Interfaces.Services
{
    public interface IAccountService
    {
        Task<ApiResponse<string>> ChangeEmailAsync(string userId, ChangeEmailRequestDTO request);

        Task<ApiResponse<string>> ChangePasswordAsync(string userId, ChangePasswordRequestDTO request);

        Task<ApiResponse<string>> ChangeNameAsync(string userId, ChangeNameRequestDTO request);
        Task<ApiResponse<MyAccountResponseDTO>> GetMyAccountAsync(string userId);
    }
}