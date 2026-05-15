using FleetFactory.Application.Features.Auth.DTOs;
using FleetFactory.Application.Interfaces.Services;
using FleetFactory.Infrastructure.Identity;
using FleetFactory.Infrastructure.Services;
using FleetFactory.Shared.Results;
using Microsoft.AspNetCore.Identity; 
using FleetFactory.Domain.Entities; 
using FleetFactory.Infrastructure.Persistence; 

namespace FleetFactory.Infrastructure.Services
{
    public class AuthService(JwtService jwtService, UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager, AppDbContext context): IAuthService
    {
        public async Task<ApiResponse<AuthResponseDto>> RegisterAsync(RegisterRequestDto request)
        {
            var user = new ApplicationUser
            {
            UserName = request.Email,
            Email = request.Email,
            FirstName = request.FirstName,
            LastName = request.LastName, 
            IsActive = true
            };

            var result = await userManager.CreateAsync(user, request.Password);
            if (!result.Succeeded)
            {
                var errors = string.Join(", ", result.Errors.Select(e => e.Description)); //can do this for the error description
                return ApiResponse<AuthResponseDto>.ErrorResponse(errors);
            }

            var role = string.IsNullOrWhiteSpace(request.Role) ? "Customer" : request.Role;

            await userManager.AddToRoleAsync(user, role); //adding the role specified by the user or fallback

           var customerProfile = new CustomerProfile
            {
                UserId = user.Id,
                FullName = $"{request.FirstName} {request.LastName}".Trim(),
                Phone = request.Phone,
                Address = request.Address,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            };

            context.CustomerProfiles.Add(customerProfile);
            await context.SaveChangesAsync();
            
            var token = await jwtService.GenerateTokenAsync(user);

            var response = new AuthResponseDto
            {
                Token = token,
                Email = user.Email!,
                Role = role
            };

            return ApiResponse<AuthResponseDto>.SuccessResponse(response, "User Registered Successfully");
        }
        public async Task<ApiResponse<AuthResponseDto>> LoginAsync(LoginRequestDto request)
        {
            var user = await userManager.FindByEmailAsync(request.Email);

            if (user == null) return ApiResponse<AuthResponseDto>.ErrorResponse("Invalid Email or Password");
            if (!user.IsActive) return ApiResponse<AuthResponseDto>.ErrorResponse("User account is inactive. Please contact support.");

            //check if password is correct
            var passwordCheck = await signInManager.CheckPasswordSignInAsync(user, request.Password, false);

            if (!passwordCheck.Succeeded) return ApiResponse<AuthResponseDto>.ErrorResponse("Invalid Email or Password");

            var roles = await userManager.GetRolesAsync(user);
            var role = roles.FirstOrDefault() ?? "Customer"; //get the first role or default to customer

            var token = await jwtService.GenerateTokenAsync(user);

            var response = new AuthResponseDto
            {
                Token = token,
                Email = user.Email!,
                Role = role
            };

            return ApiResponse<AuthResponseDto>.SuccessResponse(response, "Login Successful");
        }
    }
}