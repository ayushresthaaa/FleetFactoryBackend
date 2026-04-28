using FleetFactory.Application.Features.Staff.DTOs;
using FleetFactory.Application.Interfaces.Repositories;
using FleetFactory.Application.Interfaces.Services;
using FleetFactory.Domain.Entities;
using FleetFactory.Infrastructure.Identity;
using FleetFactory.Shared.Results;
using FleetFactory.Infrastructure.Helpers;

using Microsoft.AspNetCore.Identity;

namespace FleetFactory.Application.Features.Staff.Services
{
    public class StaffService(
        IStaffRepository _staffRepository,
        UserManager<ApplicationUser> _userManager
    ) : IStaffService
    {
        public async Task<ApiResponse<PagedResult<StaffResponseDTO>>> GetAllAsync(int pageNumber, int pageSize)
        {
            pageNumber = pageNumber < 1 ? 1 : pageNumber;
            pageSize = pageSize < 1 ? 10 : pageSize;

            var (staffs, totalCount) = await _staffRepository.GetPagedAsync(pageNumber, pageSize);

            var responseList = new List<StaffResponseDTO>();

            foreach (var staff in staffs)
            {
                var user = await _userManager.FindByIdAsync(staff.UserId);

                if (user == null) continue;

                responseList.Add(new StaffResponseDTO
                {
                    StaffProfileId = staff.Id,
                    UserId = user.Id,
                    Email = user.Email!,
                    FullName = staff.FullName,
                    Phone = staff.Phone,
                    Address = staff.Address,
                    HiredAt = staff.HiredAt,
                    IsActive = user.IsActive,
                    CreatedAt = staff.CreatedAt
                });
            }

            var paged = PagedResult<StaffResponseDTO>.Create(
                responseList,
                pageNumber,
                pageSize,
                totalCount
            );

            return ApiResponse<PagedResult<StaffResponseDTO>>.SuccessResponse(paged, "Staff retrieved");
        }

        public async Task<ApiResponse<StaffResponseDTO>> GetByIdAsync(Guid id)
        {
            var staff = await _staffRepository.GetByIdAsync(id);

            if (staff == null)
                return ApiResponse<StaffResponseDTO>.ErrorResponse("Staff not found");

            var user = await _userManager.FindByIdAsync(staff.UserId);

            if (user == null)
                return ApiResponse<StaffResponseDTO>.ErrorResponse("User not found");

            var response = new StaffResponseDTO
            {
                StaffProfileId = staff.Id,
                UserId = user.Id,
                Email = user.Email!,
                FullName = staff.FullName,
                Phone = staff.Phone,
                Address = staff.Address,
                HiredAt = staff.HiredAt,
                IsActive = user.IsActive,
                CreatedAt = staff.CreatedAt
            };

            return ApiResponse<StaffResponseDTO>.SuccessResponse(response, "Staff fetched");
        }

        public async Task<ApiResponse<StaffResponseDTO>> CreateAsync(CreateStaffRequestDTO request)
        {
            // Create Identity User
            var user = new ApplicationUser
            {
                UserName = request.Email,
                Email = request.Email,
                FirstName = request.FullName,
                IsActive = true
            };

            var result = await _userManager.CreateAsync(user, request.Password);

            if (!result.Succeeded)
            {
                var errors = string.Join(", ", result.Errors.Select(e => e.Description));
                return ApiResponse<StaffResponseDTO>.ErrorResponse(errors);
            }

            // Assign Staff role
            await _userManager.AddToRoleAsync(user, "Staff");

            // Create StaffProfile
            var staff = new StaffProfile
            {
                UserId = user.Id,
                FullName = request.FullName,
                Phone = request.Phone,
                Address = request.Address,
                HiredAt = request.HiredAt.HasValue
                        ? DateTime.SpecifyKind(request.HiredAt.Value, DateTimeKind.Utc)
                        : null,
                CreatedAt = DateTimeHelper.UtcNow,
                UpdatedAt = DateTimeHelper.UtcNow
            };

            await _staffRepository.AddAsync(staff);
            await _staffRepository.SaveChangesAsync();

            var response = new StaffResponseDTO
            {
                StaffProfileId = staff.Id,
                UserId = user.Id,
                Email = user.Email!,
                FullName = staff.FullName,
                Phone = staff.Phone,
                Address = staff.Address,
                HiredAt = staff.HiredAt,
                IsActive = user.IsActive,
                CreatedAt = staff.CreatedAt
            };

            return ApiResponse<StaffResponseDTO>.SuccessResponse(response, "Staff created successfully");
        }

        public async Task<ApiResponse<StaffResponseDTO>> UpdateAsync(Guid id, UpdateStaffRequestDTO request)
        {
            var staff = await _staffRepository.GetByIdAsync(id);

            if (staff == null)
                return ApiResponse<StaffResponseDTO>.ErrorResponse("Staff not found");

            var user = await _userManager.FindByIdAsync(staff.UserId);

            if (user == null)
                return ApiResponse<StaffResponseDTO>.ErrorResponse("User not found");

            // update staff profile
            staff.FullName = request.FullName;
            staff.Phone = request.Phone;
            staff.Address = request.Address;
            staff.HiredAt = request.HiredAt.HasValue
                ? DateTime.SpecifyKind(request.HiredAt.Value, DateTimeKind.Utc)
                : null;
            staff.UpdatedAt = DateTimeHelper.UtcNow;

            // update user active state
            user.IsActive = request.IsActive;

            _staffRepository.Update(staff);
            await _staffRepository.SaveChangesAsync();

            await _userManager.UpdateAsync(user);

            var response = new StaffResponseDTO
            {
                StaffProfileId = staff.Id,
                UserId = user.Id,
                Email = user.Email!,
                FullName = staff.FullName,
                Phone = staff.Phone,
                Address = staff.Address,
                HiredAt = staff.HiredAt,
                IsActive = user.IsActive,
                CreatedAt = staff.CreatedAt
            };

            return ApiResponse<StaffResponseDTO>.SuccessResponse(response, "Staff updated");
        }

        public async Task<ApiResponse<string>> DeactivateAsync(Guid id)
        {
            var staff = await _staffRepository.GetByIdAsync(id);

            if (staff == null)
                return ApiResponse<string>.ErrorResponse("Staff not found");

            var user = await _userManager.FindByIdAsync(staff.UserId);

            if (user == null)
                return ApiResponse<string>.ErrorResponse("User not found");

            user.IsActive = false;

            await _userManager.UpdateAsync(user);

            return ApiResponse<string>.SuccessResponse("Deactivated", "Staff deactivated");
        }
    }
}