using FleetFactory.Application.Features.LowStock.DTOs;
using FleetFactory.Application.Interfaces.Repositories;
using FleetFactory.Application.Interfaces.Services;
using FleetFactory.Domain.Entities;
using FleetFactory.Infrastructure.Helpers;
using FleetFactory.Infrastructure.Identity;
using FleetFactory.Shared.Results;
using Microsoft.AspNetCore.Identity;

namespace FleetFactory.Application.Features.LowStock.Services
{
    public class LowStockService(
        ILowStockRepository _lowStockRepository,
        IEmailService _emailService,
        UserManager<ApplicationUser> _userManager
    ) : ILowStockService
    {
        public async Task<ApiResponse<List<LowStockNotificationResponseDTO>>> 
            CheckLowStockAsync()
        {
            var lowStockParts = await _lowStockRepository.GetLowStockPartsAsync();

            Console.WriteLine($"Low stock parts found: {lowStockParts.Count}");

            var admins = await _userManager.GetUsersInRoleAsync("Admin");

            Console.WriteLine($"Admin users found: {admins.Count}");

            var response = new List<LowStockNotificationResponseDTO>();

            foreach (var part in lowStockParts)
            {
                var message =
                    $"{part.Name} is low in stock. Current quantity: {part.StockQty}";

                var notification = new Notification
                {
                    Type = "low_stock",
                    Title = "Low Stock Alert",
                    Message = message,
                    ReferenceId = part.Id,
                    IsRead = false,
                    CreatedAt = DateTimeHelper.UtcNow
                };

                await _lowStockRepository.AddNotificationAsync(notification);

                foreach (var admin in admins)
                {
                    if (string.IsNullOrWhiteSpace(admin.Email))
                    {
                        Console.WriteLine($"Admin user {admin.Id} has no email.");
                        continue;
                    }

                    Console.WriteLine($"Sending low stock email to: {admin.Email}");

                    await _emailService.SendLowStockAlertEmailAsync(
                        admin.Email,
                        part.Name,
                        part.Sku,
                        part.StockQty,
                        part.Category!.LowStockThreshold
                    );
                }

                response.Add(new LowStockNotificationResponseDTO
                {
                    PartId = part.Id,
                    Sku = part.Sku,
                    PartName = part.Name,
                    StockQty = part.StockQty,
                    Message = message
                });
            }

            await _lowStockRepository.SaveChangesAsync();

            return ApiResponse<List<LowStockNotificationResponseDTO>>
                .SuccessResponse(response, "Low stock check completed");
        }
    }
}