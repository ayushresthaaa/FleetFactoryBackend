using FleetFactory.Application.Features.OverdueCredits.DTOs;
using FleetFactory.Application.Interfaces.Repositories;
using FleetFactory.Application.Interfaces.Services;
using FleetFactory.Domain.Entities;
using FleetFactory.Infrastructure.Helpers;
using FleetFactory.Shared.Results;

namespace FleetFactory.Application.Features.OverdueCredits.Services
{
    public class OverdueCreditService(
        IOverdueCreditRepository _overdueCreditRepository,
        IEmailService _emailService
    ) : IOverdueCreditService
    {
        public async Task<ApiResponse<List<OverdueCreditCustomerDTO>>> GetOverdueCreditsAsync()
        {
            var thresholdDate = DateTimeHelper.UtcNow.AddMonths(-1);

            var customers = await _overdueCreditRepository
                .GetOverdueCreditCustomersAsync(thresholdDate);

            var response = customers.Select(c => new OverdueCreditCustomerDTO
            {
                CustomerId = c.Id,
                CustomerName = c.FullName,
                Email = c.User.Email,
                Phone = c.Phone,
                CreditBalance = c.CreditBalance,
                LastUpdatedAt = c.UpdatedAt
            }).ToList();

            return ApiResponse<List<OverdueCreditCustomerDTO>>
                .SuccessResponse(response, "Overdue credit customers loaded successfully");
        }

        public async Task<ApiResponse<string>> SendReminderToCustomerAsync(Guid customerId)
        {
            var thresholdDate = DateTimeHelper.UtcNow.AddMonths(-1);

            var customer = await _overdueCreditRepository
                .GetOverdueCreditCustomerByIdAsync(customerId, thresholdDate);

            if (customer == null)
            {
                return ApiResponse<string>
                    .ErrorResponse("Customer not found or does not have overdue credit.");
            }

            if (string.IsNullOrWhiteSpace(customer.User.Email))
            {
                return ApiResponse<string>
                    .ErrorResponse("Customer does not have an email address.");
            }

            await SendReminderAsync(customer);

            await _overdueCreditRepository.SaveChangesAsync();

            return ApiResponse<string>
                .SuccessResponse("Reminder sent", "Overdue credit reminder sent successfully");
        }

        public async Task<ApiResponse<int>> SendReminderToAllAsync()
        {
            var thresholdDate = DateTimeHelper.UtcNow.AddMonths(-1);

            var customers = await _overdueCreditRepository
                .GetOverdueCreditCustomersAsync(thresholdDate);

            var sentCount = 0;

            foreach (var customer in customers)
            {
                if (string.IsNullOrWhiteSpace(customer.User.Email))
                    continue;

                await SendReminderAsync(customer);
                sentCount++;
            }

            await _overdueCreditRepository.SaveChangesAsync();

            return ApiResponse<int>
                .SuccessResponse(sentCount, "Overdue credit reminders sent successfully");
        }

        private async Task SendReminderAsync(CustomerProfile customer)
        {
            await _emailService.SendOverdueCreditReminderEmailAsync(
                customer.User.Email!,
                customer.FullName,
                customer.CreditBalance
            );

            var notification = new Notification
            {
                UserId = customer.UserId,
                Type = "overdue_credit",
                Title = "Overdue Credit Reminder",
                Message = $"You have pending credit of Rs. {customer.CreditBalance}. Please clear your payment.",
                ReferenceId = customer.Id,
                IsRead = false,
                CreatedAt = DateTimeHelper.UtcNow
            };

            await _overdueCreditRepository.AddNotificationAsync(notification);
        }
    }
}