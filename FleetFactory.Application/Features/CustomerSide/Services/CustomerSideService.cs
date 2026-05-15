using FleetFactory.Application.Features.CustomerSide.DTOs;
using FleetFactory.Application.Interfaces.Repositories;
using FleetFactory.Application.Interfaces.Services;
using FleetFactory.Shared.Results;

namespace FleetFactory.Application.Features.CustomerSide.Services
{
    public class CustomerSideService(ICustomerSideRepository _customerSideRepository)
        : ICustomerSideService
    {
        public async Task<ApiResponse<List<CustomerPurchaseHistoryResponseDto>>>
            GetMyPurchaseHistoryAsync(string userId)
        {
            var customer = await _customerSideRepository.GetCustomerByUserIdAsync(userId);

            if (customer == null)
                return ApiResponse<List<CustomerPurchaseHistoryResponseDto>>
                    .ErrorResponse("Customer profile not found");

            var invoices = await _customerSideRepository.GetPurchaseHistoryAsync(customer.Id);

            var response = invoices.Select(s => new CustomerPurchaseHistoryResponseDto
            {
                SalesInvoiceId = s.Id,
                InvoiceNo = s.InvoiceNo,
                Status = s.Status,
                TotalAmount = s.TotalAmount,
                CreatedAt = s.CreatedAt,
                VehicleNumber = s.Vehicle?.VehicleNumber,
                ItemCount = s.Items?.Count ?? 0
            }).ToList();

            return ApiResponse<List<CustomerPurchaseHistoryResponseDto>>
                .SuccessResponse(response, "Purchase history retrieved successfully");
        }

        public async Task<ApiResponse<List<CustomerAppointmentHistoryResponseDto>>>
            GetMyAppointmentHistoryAsync(string userId)
        {
            var customer = await _customerSideRepository.GetCustomerByUserIdAsync(userId);

            if (customer == null)
                return ApiResponse<List<CustomerAppointmentHistoryResponseDto>>
                    .ErrorResponse("Customer profile not found");

            var appointments = await _customerSideRepository.GetAppointmentHistoryAsync(customer.Id);

            var response = appointments.Select(a => new CustomerAppointmentHistoryResponseDto
            {
                AppointmentId = a.Id,
                VehicleNumber = a.Vehicle?.VehicleNumber,
                ScheduledAt = a.ScheduledAt,
                Status = a.Status.ToString(),
                Notes = a.Notes
            }).ToList();

            return ApiResponse<List<CustomerAppointmentHistoryResponseDto>>
                .SuccessResponse(response, "Appointment history retrieved successfully");
        }
    }
}