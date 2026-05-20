using FleetFactory.Application.Features.Account.DTOs;
using FleetFactory.Application.Interfaces.Services;
using FleetFactory.Infrastructure.Helpers;
using FleetFactory.Infrastructure.Identity;
using FleetFactory.Shared.Results;
using Microsoft.AspNetCore.Identity;
using FleetFactory.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
namespace FleetFactory.Application.Features.Account.Services
{
    public class AccountService(
        UserManager<ApplicationUser> _userManager,
        AppDbContext _context
    ) : IAccountService
    {
        public async Task<ApiResponse<string>> ChangeEmailAsync(
            string userId,
            ChangeEmailRequestDTO request)
        {
            if (string.IsNullOrWhiteSpace(request.NewEmail))
                return ApiResponse<string>.ErrorResponse("Email is required");

            var user = await _userManager.FindByIdAsync(userId);

            if (user == null)
                return ApiResponse<string>.ErrorResponse("User not found");

            var existingUser = await _userManager.FindByEmailAsync(request.NewEmail);

            if (existingUser != null && existingUser.Id != user.Id)
                return ApiResponse<string>.ErrorResponse("Email is already in use");

            user.Email = request.NewEmail;
            user.UserName = request.NewEmail;
            user.NormalizedEmail = request.NewEmail.ToUpper();
            user.NormalizedUserName = request.NewEmail.ToUpper();
            user.UpdatedAt = DateTimeHelper.UtcNow;

            var result = await _userManager.UpdateAsync(user);

            if (!result.Succeeded)
                return ApiResponse<string>.ErrorResponse(
                    string.Join(", ", result.Errors.Select(e => e.Description))
                );

            return ApiResponse<string>
                .SuccessResponse("Email updated", "Email changed successfully");
        }

        public async Task<ApiResponse<string>> ChangePasswordAsync(
            string userId,
            ChangePasswordRequestDTO request)
        {
            if (string.IsNullOrWhiteSpace(request.CurrentPassword))
                return ApiResponse<string>.ErrorResponse("Current password is required");

            if (string.IsNullOrWhiteSpace(request.NewPassword))
                return ApiResponse<string>.ErrorResponse("New password is required");

            var user = await _userManager.FindByIdAsync(userId);

            if (user == null)
                return ApiResponse<string>.ErrorResponse("User not found");

            var result = await _userManager.ChangePasswordAsync(
                user,
                request.CurrentPassword,
                request.NewPassword
            );

            if (!result.Succeeded)
                return ApiResponse<string>.ErrorResponse(
                    string.Join(", ", result.Errors.Select(e => e.Description))
                );

            user.UpdatedAt = DateTimeHelper.UtcNow;
            await _userManager.UpdateAsync(user);

            return ApiResponse<string>
                .SuccessResponse("Password updated", "Password changed successfully");
        }

        public async Task<ApiResponse<string>> ChangeNameAsync(
            string userId,
            ChangeNameRequestDTO request)
        {
            if (string.IsNullOrWhiteSpace(request.FirstName))
                return ApiResponse<string>.ErrorResponse("First name is required");

            if (string.IsNullOrWhiteSpace(request.LastName))
                return ApiResponse<string>.ErrorResponse("Last name is required");

            var user = await _userManager.FindByIdAsync(userId);

            if (user == null)
                return ApiResponse<string>.ErrorResponse("User not found");

            user.FirstName = request.FirstName;
            user.LastName = request.LastName;
            user.UpdatedAt = DateTimeHelper.UtcNow;

            var fullName = $"{request.FirstName} {request.LastName}".Trim();

            var customerProfile = await _context.CustomerProfiles
                .FirstOrDefaultAsync(c => c.UserId == userId);

            if (customerProfile != null)
            {
                customerProfile.FullName = fullName;
                customerProfile.UpdatedAt = DateTimeHelper.UtcNow;
            }

            var staffProfile = await _context.StaffProfiles
                .FirstOrDefaultAsync(s => s.UserId == userId);

            if (staffProfile != null)
            {
                staffProfile.FullName = fullName;
                staffProfile.UpdatedAt = DateTimeHelper.UtcNow;
            }

            var result = await _userManager.UpdateAsync(user);

            if (!result.Succeeded)
            {
                return ApiResponse<string>.ErrorResponse(
                    string.Join(", ", result.Errors.Select(e => e.Description))
                );
            }

            await _context.SaveChangesAsync();

            return ApiResponse<string>
                .SuccessResponse("Name updated", "Name changed successfully");
        }

        public async Task<ApiResponse<MyAccountResponseDTO>> GetMyAccountAsync(string userId)
        {
            var user = await _userManager.FindByIdAsync(userId);

            if (user == null)
                return ApiResponse<MyAccountResponseDTO>.ErrorResponse("User not found");

            return ApiResponse<MyAccountResponseDTO>.SuccessResponse(
                new MyAccountResponseDTO
                {
                    Email = user.Email ?? "",
                    FirstName = user.FirstName,
                    LastName = user.LastName
                },
                "Account retrieved successfully"
            );
        }
    }
}