using FleetFactory.Application.Features.Staff.DTOs;
using FleetFactory.Application.Interfaces.Services;
using FleetFactory.Domain.Entities;
using FleetFactory.Infrastructure.Identity;
using FleetFactory.Infrastructure.Persistence;
using FleetFactory.Shared.Results;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace FleetFactory.Application.Features.Staff.Services
{

    public class StaffService(
        UserManager<ApplicationUser> _userManager,
        AppDbContext _context) : IStaffService
    {
        public async Task<ApiResponse<StaffResponseDto>> RegisterStaffAsync(RegisterStaffRequestDto request)
        {
            //only Staff or Admin allowed here
            if (request.Role != "Staff" && request.Role != "Admin")
                return ApiResponse<StaffResponseDto>.ErrorResponse("Role must be 'Staff' or 'Admin'.");

            //check email uniqueness (UserManager handles the Users table)
            var existing = await _userManager.FindByEmailAsync(request.Email);
            if (existing != null)
                return ApiResponse<StaffResponseDto>.ErrorResponse($"Email '{request.Email}' is already registered.");

            //create ApplicationUser via Identity (handles password hashing)

            var user = new ApplicationUser
            {
                UserName  = request.Email,
                Email     = request.Email,
                FirstName = request.FirstName,
                LastName  = request.LastName,
                IsActive  = true
            };

            var createResult = await _userManager.CreateAsync(user, request.Password);
            if (!createResult.Succeeded)
            {
                var errors = string.Join(", ", createResult.Errors.Select(e => e.Description));
                return ApiResponse<StaffResponseDto>.ErrorResponse(errors);
            }

            //assign role (Staff or Admin)
            var roleResult = await _userManager.AddToRoleAsync(user, request.Role);
            if (!roleResult.Succeeded)
            {
                await _userManager.DeleteAsync(user); // rollback
                var errors = string.Join(", ", roleResult.Errors.Select(e => e.Description));
                return ApiResponse<StaffResponseDto>.ErrorResponse(errors);
            }

            //create StaffProfile
            var profile = new StaffProfile
            {
                UserId   = user.Id,   //user.Id is string (Identity)
                FullName = $"{request.FirstName} {request.LastName}".Trim(),
                Phone    = request.Phone,
                Address  = request.Address,
                HiredAt  = request.HiredAt
            };

            await _context.StaffProfiles.AddAsync(profile);
            await _context.SaveChangesAsync();

            return ApiResponse<StaffResponseDto>.SuccessResponse(
                MapToResponse(user, profile, request.Role),
                "Staff registered successfully."
            );
        }

        public async Task<ApiResponse<List<StaffSummaryDto>>> GetAllStaffAsync()
        {
            //get IDs of all Staff and Admin users from Identity
            var staffUsers = await _userManager.GetUsersInRoleAsync("Staff");
            var adminUsers = await _userManager.GetUsersInRoleAsync("Admin");

            var allUsers = staffUsers.Concat(adminUsers)
                                     .DistinctBy(u => u.Id)
                                     .ToList();

            var userIds = allUsers.Select(u => u.Id).ToList();

            //load StaffProfiles for those users 
            var profiles = await _context.StaffProfiles
                .Where(sp => userIds.Contains(sp.UserId))
                .ToListAsync();

            var profileMap = profiles.ToDictionary(sp => sp.UserId);

            var summaries = new List<StaffSummaryDto>();

            foreach (var user in allUsers.OrderBy(u => u.FirstName))
            {
                var roles = await _userManager.GetRolesAsync(user);
                var role  = roles.FirstOrDefault() ?? "Staff";

                profileMap.TryGetValue(user.Id, out var profile);

                summaries.Add(new StaffSummaryDto
                {
                    UserId   = user.Id,
                    FullName = profile?.FullName ?? $"{user.FirstName} {user.LastName}",
                    Email    = user.Email!,
                    Phone    = profile?.Phone,
                    Role     = role,
                    IsActive = user.IsActive,
                    HiredAt  = profile?.HiredAt
                });
            }

            return ApiResponse<List<StaffSummaryDto>>.SuccessResponse(
                summaries,
                "Staff list retrieved successfully."
            );
        }

        public async Task<ApiResponse<StaffResponseDto>> GetStaffByIdAsync(string userId)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
                return ApiResponse<StaffResponseDto>.ErrorResponse("Staff member not found.");

            var roles = await _userManager.GetRolesAsync(user);
            var role  = roles.FirstOrDefault() ?? "Staff";

            // only return if Staff or Admin
            if (role != "Staff" && role != "Admin")
                return ApiResponse<StaffResponseDto>.ErrorResponse("Staff member not found.");

            var profile = await _context.StaffProfiles
                .FirstOrDefaultAsync(sp => sp.UserId == userId);

            return ApiResponse<StaffResponseDto>.SuccessResponse(
                MapToResponse(user, profile, role),
                "Staff member retrieved."
            );
        }

        public async Task<ApiResponse<StaffResponseDto>> UpdateStaffAsync(string userId, UpdateStaffRequestDto request)
        {
            if (request.Role != "Staff" && request.Role != "Admin")
                return ApiResponse<StaffResponseDto>.ErrorResponse("Role must be 'Staff' or 'Admin'.");

            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
                return ApiResponse<StaffResponseDto>.ErrorResponse("Staff member not found.");

            //update Identity user fields
            user.FirstName = request.FirstName;
            user.LastName  = request.LastName;
            user.UpdatedAt = DateTime.UtcNow;
            await _userManager.UpdateAsync(user);

            //update role if changed
            var currentRoles = await _userManager.GetRolesAsync(user);
            var currentRole  = currentRoles.FirstOrDefault() ?? "Staff";
            if (currentRole != request.Role)
            {
                await _userManager.RemoveFromRoleAsync(user, currentRole);
                await _userManager.AddToRoleAsync(user, request.Role);
            }

            //update StaffProfile
            var profile = await _context.StaffProfiles
                .FirstOrDefaultAsync(sp => sp.UserId == userId);

            if (profile != null)
            {
                profile.FullName  = $"{request.FirstName} {request.LastName}".Trim();
                profile.Phone     = request.Phone;
                profile.Address   = request.Address;
                profile.HiredAt   = request.HiredAt;
                profile.UpdatedAt = DateTime.UtcNow;
                _context.StaffProfiles.Update(profile);
                await _context.SaveChangesAsync();
            }

            return ApiResponse<StaffResponseDto>.SuccessResponse(
                MapToResponse(user, profile, request.Role),
                "Staff updated successfully."
            );
        }

        public async Task<ApiResponse<StaffResponseDto>> SetStaffStatusAsync(string userId, SetStaffStatusRequestDto request)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
                return ApiResponse<StaffResponseDto>.ErrorResponse("Staff member not found.");

            user.IsActive  = request.IsActive;
            user.UpdatedAt = DateTime.UtcNow;
            await _userManager.UpdateAsync(user);

            var roles = await _userManager.GetRolesAsync(user);
            var role  = roles.FirstOrDefault() ?? "Staff";

            var profile = await _context.StaffProfiles
                .FirstOrDefaultAsync(sp => sp.UserId == userId);

            var action = request.IsActive ? "activated" : "deactivated";
            return ApiResponse<StaffResponseDto>.SuccessResponse(
                MapToResponse(user, profile, role),
                $"Staff account {action} successfully."
            );
        }

        //maps ApplicationUser + StaffProfile = response DTO
        private static StaffResponseDto MapToResponse(
            ApplicationUser user,
            StaffProfile?   profile,
            string          role) => new()
        {
            UserId         = user.Id,
            StaffProfileId = profile?.Id ?? Guid.Empty,
            Email          = user.Email!,
            FirstName      = user.FirstName,
            LastName       = user.LastName,
            FullName       = profile?.FullName ?? $"{user.FirstName} {user.LastName}",
            Phone          = profile?.Phone,
            Address        = profile?.Address,
            HiredAt        = profile?.HiredAt,
            Role           = role,
            IsActive       = user.IsActive,
            CreatedAt      = user.CreatedAt
        };
    }
}
