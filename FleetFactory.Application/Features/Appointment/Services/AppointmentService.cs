using FleetFactory.Application.Features.Appointments.DTOs;
using FleetFactory.Application.Interfaces.Repositories;
using FleetFactory.Application.Interfaces.Services;
using FleetFactory.Domain.Entities;
using FleetFactory.Domain.Enums;
using FleetFactory.Infrastructure.Helpers;
using FleetFactory.Shared.Results;

namespace FleetFactory.Application.Features.Appointments.Services
{
    public class AppointmentService(
        IAppointmentRepository _appointmentRepository,
        ICustomerProfileRepository _customerProfileRepository,
        IEmailService _emailService,
        INotificationService _notificationService
    ) : IAppointmentService
    {
        public async Task<ApiResponse<PagedResult<AppointmentResponseDTO>>> GetAllAsync(
            int pageNumber,
            int pageSize)
        {
            pageNumber = pageNumber < 1 ? 1 : pageNumber;
            pageSize = pageSize < 1 ? 10 : pageSize;

            var (appointments, totalCount) =
                await _appointmentRepository.GetPagedAsync(pageNumber, pageSize);

            var response = appointments.Select(MapToResponse).ToList();

            var paged = PagedResult<AppointmentResponseDTO>.Create(
                response,
                pageNumber,
                pageSize,
                totalCount
            );

            return ApiResponse<PagedResult<AppointmentResponseDTO>>
                .SuccessResponse(paged, "Appointments retrieved successfully");
        }

        public async Task<ApiResponse<AppointmentResponseDTO>> GetByIdAsync(Guid id)
        {
            var appointment = await _appointmentRepository.GetByIdAsync(id);

            if (appointment == null)
                return ApiResponse<AppointmentResponseDTO>.ErrorResponse("Appointment not found");

            return ApiResponse<AppointmentResponseDTO>
                .SuccessResponse(MapToResponse(appointment), "Appointment fetched successfully");
        }

        public async Task<ApiResponse<AppointmentResponseDTO>> CreateMyAppointmentAsync(
            string userId,
            CreateMyAppointmentRequestDTO request)
        {
            var customer = await _customerProfileRepository
                .GetByUserIdWithVehiclesAsync(userId);

            if (customer == null)
                return ApiResponse<AppointmentResponseDTO>.ErrorResponse("Customer profile not found");
            var scheduledAtUtc = request.ScheduledAt.Kind == DateTimeKind.Utc
                ? request.ScheduledAt
                : request.ScheduledAt.ToUniversalTime();

            if (scheduledAtUtc <= DateTimeHelper.UtcNow)
                return ApiResponse<AppointmentResponseDTO>.ErrorResponse("Scheduled date must be in the future");

            var appointmentCount = await _appointmentRepository
                .CountActiveAppointmentsByDateAsync(scheduledAtUtc);
            if (appointmentCount >= 30)
            {
                return ApiResponse<AppointmentResponseDTO>
                    .ErrorResponse("Appointment limit reached for this day. Please choose another day.");
            }

            if (request.VehicleId.HasValue &&
                !customer.Vehicles.Any(v => v.Id == request.VehicleId.Value))
            {
                return ApiResponse<AppointmentResponseDTO>
                    .ErrorResponse("Vehicle does not belong to this customer");
            }

            var appointment = new Appointment
            {
                CustomerId = customer.Id,
                VehicleId = request.VehicleId,
                ScheduledAt = scheduledAtUtc,
                Notes = request.Notes,
                Status = AppointmentStatus.Pending,
                CreatedAt = DateTimeHelper.UtcNow,
                UpdatedAt = DateTimeHelper.UtcNow
            };

            await _appointmentRepository.AddAsync(appointment);
            await _appointmentRepository.SaveChangesAsync();

            return await GetByIdAsync(appointment.Id);
        }

        public async Task<ApiResponse<AppointmentResponseDTO>> ConfirmAsync(Guid id)
        {
            var appointment = await _appointmentRepository.GetByIdAsync(id);

            if (appointment == null)
                return ApiResponse<AppointmentResponseDTO>.ErrorResponse("Appointment not found");

            if (appointment.Status == AppointmentStatus.Cancelled)
                return ApiResponse<AppointmentResponseDTO>.ErrorResponse("Cancelled appointment cannot be confirmed");

            if (appointment.Status == AppointmentStatus.Completed)
                return ApiResponse<AppointmentResponseDTO>.ErrorResponse("Completed appointment cannot be confirmed");

            appointment.Status = AppointmentStatus.Confirmed;
            appointment.UpdatedAt = DateTimeHelper.UtcNow;

            _appointmentRepository.Update(appointment);
            await _appointmentRepository.SaveChangesAsync();

            await _notificationService.CreateAsync(
                appointment.Customer.UserId,
                "appointment_confirmed",
                "Appointment Confirmed",
                $"Your appointment on {appointment.ScheduledAt} has been confirmed.",
                appointment.Id
            );
            return await GetByIdAsync(id);
        }

        public async Task<ApiResponse<AppointmentResponseDTO>> CancelAsync(Guid id, CancelAppointmentRequestDTO request)
        {
            var appointment = await _appointmentRepository.GetByIdAsync(id);

            if (appointment == null)
                return ApiResponse<AppointmentResponseDTO>.ErrorResponse("Appointment not found");

            if (appointment.Status == AppointmentStatus.Completed)
                return ApiResponse<AppointmentResponseDTO>.ErrorResponse("Completed appointment cannot be cancelled");

            if (appointment.Status == AppointmentStatus.Cancelled)
                return ApiResponse<AppointmentResponseDTO>.ErrorResponse("Appointment is already cancelled");

            appointment.Status = AppointmentStatus.Cancelled;
            appointment.UpdatedAt = DateTimeHelper.UtcNow;

            _appointmentRepository.Update(appointment);
            await _appointmentRepository.SaveChangesAsync();

            var customerEmail = appointment.Customer.User?.Email;
            var customerName = appointment.Customer.FullName;

            if (!string.IsNullOrWhiteSpace(customerEmail))
            {
                await _emailService.SendAppointmentCancelledEmailAsync(
                    customerEmail,
                    customerName,
                    appointment.ScheduledAt,
                    request.Reason
                );
            }

            await _notificationService.CreateAsync(
                appointment.Customer.UserId,
                "appointment_cancelled",
                "Appointment Cancelled",
                $"Your appointment on {appointment.ScheduledAt} has been cancelled.",
                appointment.Id
            );
            return await GetByIdAsync(id);
        }

        private static AppointmentResponseDTO MapToResponse(Appointment appointment)
        {
            return new AppointmentResponseDTO
            {
                Id = appointment.Id,
                CustomerId = appointment.CustomerId,
                CustomerName = appointment.Customer?.FullName ?? "",
                VehicleId = appointment.VehicleId,
                VehicleNumber = appointment.Vehicle?.VehicleNumber,
                ScheduledAt = appointment.ScheduledAt,
                Status = appointment.Status.ToString(),
                Notes = appointment.Notes,
                CreatedAt = appointment.CreatedAt
            };
        }


        public async Task<ApiResponse<PagedResult<AppointmentResponseDTO>>> SearchAsync(
            string? query,
            AppointmentStatus? status,
            int pageNumber,
            int pageSize)
        {
            pageNumber = pageNumber < 1 ? 1 : pageNumber;
            pageSize = pageSize < 1 ? 10 : pageSize;

            var (appointments, totalCount) =
                await _appointmentRepository.SearchAsync(
                    query,
                    status,
                    pageNumber,
                    pageSize);

            var response = appointments.Select(MapToResponse).ToList();

            var paged = PagedResult<AppointmentResponseDTO>.Create(
                response,
                pageNumber,
                pageSize,
                totalCount
            );

            return ApiResponse<PagedResult<AppointmentResponseDTO>>
                .SuccessResponse(paged, "Appointment search completed");
        }
    }
}