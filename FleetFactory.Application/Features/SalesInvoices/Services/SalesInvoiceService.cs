using FleetFactory.Application.Features.SalesInvoices.DTOs;
using FleetFactory.Application.Interfaces.Repositories;
using FleetFactory.Application.Interfaces.Services;
using FleetFactory.Domain.Entities;
using FleetFactory.Domain.Enums;
using FleetFactory.Infrastructure.Helpers;
using FleetFactory.Shared.Results;

namespace FleetFactory.Application.Features.SalesInvoices.Services
{
    public class SalesInvoiceService(
        ISalesInvoiceRepository _salesInvoiceRepository,
        IPartRepository _partRepository,
        ICustomerRepository _customerRepository
    ) : ISalesInvoiceService
    {
        public async Task<ApiResponse<PagedResult<SalesInvoiceResponseDto>>> GetAllAsync(int pageNumber, int pageSize)
        {
            pageNumber = pageNumber < 1 ? 1 : pageNumber;
            pageSize = pageSize < 1 ? 10 : pageSize;

            var (invoices, totalCount) = await _salesInvoiceRepository.GetPagedAsync(pageNumber, pageSize);

            var response = invoices.Select(MapToResponse).ToList();

            var pagedResult = PagedResult<SalesInvoiceResponseDto>.Create(
                response,
                pageNumber,
                pageSize,
                totalCount
            );

            return ApiResponse<PagedResult<SalesInvoiceResponseDto>>
                .SuccessResponse(pagedResult, "Sales invoices retrieved successfully");
        }

        public async Task<ApiResponse<SalesInvoiceResponseDto>> GetByIdAsync(Guid id)
        {
            var invoice = await _salesInvoiceRepository.GetByIdAsync(id);

            if (invoice == null)
                return ApiResponse<SalesInvoiceResponseDto>.ErrorResponse("Sales invoice not found");

            return ApiResponse<SalesInvoiceResponseDto>
                .SuccessResponse(MapToResponse(invoice), "Sales invoice fetched successfully");
        }

        public async Task<ApiResponse<SalesInvoiceResponseDto>> CreateAsync(
            CreateSalesInvoiceRequestDto request,
            string createdById)
        {
            if ((request.Items == null || request.Items.Count == 0) && request.ServiceCharge <= 0)
                return ApiResponse<SalesInvoiceResponseDto>
                    .ErrorResponse("Invoice must contain at least one part item or service charge");

            var invoiceNo = $"SINV-{DateTime.UtcNow:yyyyMMddHHmmss}";

            var customer = await _customerRepository.GetByIdAsync(request.CustomerId);

            if (customer == null)
                return ApiResponse<SalesInvoiceResponseDto>
                    .ErrorResponse("Customer not found");
            if (request.VehicleId.HasValue &&
                !customer.Vehicles.Any(v => v.Id == request.VehicleId.Value))
            {
                return ApiResponse<SalesInvoiceResponseDto>
                    .ErrorResponse("Vehicle does not belong to this customer");
            }
            
            //validate appointment if provided
            Appointment? appointment = null;

            if (request.AppointmentId.HasValue)
            {
                appointment = await _salesInvoiceRepository.GetAppointmentByIdAsync(request.AppointmentId.Value);

                if (appointment == null)
                    return ApiResponse<SalesInvoiceResponseDto>
                        .ErrorResponse("Appointment not found");

                if (appointment.CustomerId != request.CustomerId)
                    return ApiResponse<SalesInvoiceResponseDto>
                        .ErrorResponse("Appointment does not belong to this customer");

                if (request.VehicleId.HasValue && appointment.VehicleId != request.VehicleId)
                    return ApiResponse<SalesInvoiceResponseDto>
                        .ErrorResponse("Appointment vehicle does not match selected vehicle");
            }


            var invoice = new SalesInvoice
            {
                CustomerId = request.CustomerId,
                VehicleId = request.VehicleId,
                AppointmentId = request.AppointmentId,
                InvoiceNo = invoiceNo,
                PaymentMethod = request.PaymentMethod,
                ServiceCharge = request.ServiceCharge,
                ServiceDescription = request.ServiceDescription,
                DueDate = request.DueDate,
                Status = request.PaymentMethod == PaymentMethod.Credit
                    ? InvoiceStatus.Pending
                    : InvoiceStatus.Paid,
                PaidAt = request.PaymentMethod == PaymentMethod.Credit
                    ? null
                    : DateTimeHelper.UtcNow,
                CreatedById = createdById,
                CreatedAt = DateTimeHelper.UtcNow,
                UpdatedAt = DateTimeHelper.UtcNow
            };

            decimal partsTotal = 0;

            foreach (var item in request.Items!)
            {
                var part = await _partRepository.GetByIdAsync(item.PartId);

                if (part == null)
                    return ApiResponse<SalesInvoiceResponseDto>
                        .ErrorResponse($"Part not found: {item.PartId}");

                if (item.Quantity <= 0)
                    return ApiResponse<SalesInvoiceResponseDto>
                        .ErrorResponse("Item quantity must be greater than 0");

                if (part.StockQty < item.Quantity)
                    return ApiResponse<SalesInvoiceResponseDto>
                        .ErrorResponse($"Not enough stock for {part.Name}");

                var unitPrice = part.UnitPrice;
                var lineTotal = item.Quantity * unitPrice;

                partsTotal += lineTotal;

                invoice.Items.Add(new SalesInvoiceItem
                {
                    PartId = part.Id,
                    Quantity = item.Quantity,
                    UnitPrice = unitPrice
                });

                part.StockQty -= item.Quantity;
                part.UpdatedAt = DateTimeHelper.UtcNow;

                await _partRepository.AddStockMovementAsync(new StockMovement
                {
                    PartId = part.Id,
                    MovementType = StockMovementType.Sale,
                    Quantity = -item.Quantity,
                    ReferenceId = invoice.Id,
                    Note = $"Stock sold from sales invoice {invoice.InvoiceNo}",
                    CreatedById = createdById,
                    CreatedAt = DateTimeHelper.UtcNow
                });

                _partRepository.Update(part);
            }

            invoice.Subtotal = partsTotal + request.ServiceCharge;

            if (invoice.Subtotal > 5000)
            {
                invoice.DiscountPct = 10;
                invoice.DiscountReason = "loyalty_5000";
            }

            var discountAmount = invoice.Subtotal * (invoice.DiscountPct / 100);
            invoice.TotalAmount = invoice.Subtotal - discountAmount;

            //mark appointment as completed if linked
            if (appointment != null)
                {
                    appointment.Status = AppointmentStatus.Completed;
                    appointment.UpdatedAt = DateTimeHelper.UtcNow;
                    _salesInvoiceRepository.UpdateAppointment(appointment);
                }

            await _salesInvoiceRepository.AddAsync(invoice);
            await _salesInvoiceRepository.SaveChangesAsync();

            return await GetByIdAsync(invoice.Id);
        }

        public async Task<ApiResponse<SalesInvoiceResponseDto>> MarkPaidAsync(Guid id)
        {
            var invoice = await _salesInvoiceRepository.GetByIdAsync(id);

            if (invoice == null)
                return ApiResponse<SalesInvoiceResponseDto>.ErrorResponse("Sales invoice not found");

            if (invoice.Status == InvoiceStatus.Paid)
                return ApiResponse<SalesInvoiceResponseDto>.ErrorResponse("Invoice is already paid");

            if (invoice.Status == InvoiceStatus.Cancelled)
                return ApiResponse<SalesInvoiceResponseDto>.ErrorResponse("Cancelled invoice cannot be paid");

            invoice.Status = InvoiceStatus.Paid;
            invoice.PaidAt = DateTimeHelper.UtcNow;
            invoice.UpdatedAt = DateTimeHelper.UtcNow;

            _salesInvoiceRepository.Update(invoice);
            await _salesInvoiceRepository.SaveChangesAsync();

            return await GetByIdAsync(id);
        }

        public async Task<ApiResponse<SalesInvoiceResponseDto>> CancelAsync(Guid id)
        {
            var invoice = await _salesInvoiceRepository.GetByIdAsync(id);

            if (invoice == null)
                return ApiResponse<SalesInvoiceResponseDto>.ErrorResponse("Sales invoice not found");

            if (invoice.Status == InvoiceStatus.Cancelled)
                return ApiResponse<SalesInvoiceResponseDto>.ErrorResponse("Invoice is already cancelled");

            if (invoice.Status == InvoiceStatus.Paid)
                return ApiResponse<SalesInvoiceResponseDto>.ErrorResponse("Paid invoice cannot be cancelled");

            invoice.Status = InvoiceStatus.Cancelled;
            invoice.UpdatedAt = DateTimeHelper.UtcNow;

            _salesInvoiceRepository.Update(invoice);
            await _salesInvoiceRepository.SaveChangesAsync();

            return await GetByIdAsync(id);
        }

        private static SalesInvoiceResponseDto MapToResponse(SalesInvoice invoice)
        {
            return new SalesInvoiceResponseDto
            {
                Id = invoice.Id,
                InvoiceNo = invoice.InvoiceNo,
                CustomerId = invoice.CustomerId,
                CustomerName = invoice.Customer?.FullName ?? "",
                VehicleId = invoice.VehicleId,
                VehicleNumber = invoice.Vehicle?.VehicleNumber,
                AppointmentId = invoice.AppointmentId,
                Status = invoice.Status.ToString(),
                PaymentMethod = invoice.PaymentMethod?.ToString(),
                ServiceCharge = invoice.ServiceCharge,
                ServiceDescription = invoice.ServiceDescription,
                Subtotal = invoice.Subtotal,
                DiscountPct = invoice.DiscountPct,
                DiscountReason = invoice.DiscountReason,
                TotalAmount = invoice.TotalAmount,
                DueDate = invoice.DueDate,
                PaidAt = invoice.PaidAt,
                CreatedAt = invoice.CreatedAt,
                Items = invoice.Items.Select(i => new SalesInvoiceItemResponseDto
                {
                    Id = i.Id,
                    PartId = i.PartId,
                    PartName = i.Part?.Name ?? "",
                    Quantity = i.Quantity,
                    UnitPrice = i.UnitPrice,
                    Subtotal = i.Quantity * i.UnitPrice
                }).ToList()
            };
        }
        public async Task<ApiResponse<PagedResult<SalesInvoiceResponseDto>>> SearchAsync(
            string? query,
            InvoiceStatus? status,
            SalesInvoiceMode? mode,
            int pageNumber,
            int pageSize)
        {
            pageNumber = pageNumber < 1 ? 1 : pageNumber;
            pageSize = pageSize < 1 ? 10 : pageSize;

            var (invoices, totalCount) = await _salesInvoiceRepository.SearchAsync(
                query,
                status,
                mode,
                pageNumber,
                pageSize);

            var response = invoices.Select(MapToResponse).ToList();

            var pagedResult = PagedResult<SalesInvoiceResponseDto>.Create(
                response,
                pageNumber,
                pageSize,
                totalCount
            );

            return ApiResponse<PagedResult<SalesInvoiceResponseDto>>
                .SuccessResponse(pagedResult, "Sales invoice search completed");
        }

        public async Task<ApiResponse<List<CustomerAppointmentForInvoiceDto>>> GetCustomerAppointmentsAsync(Guid customerId)
        {
            var customer = await _customerRepository.GetByIdAsync(customerId);

            if (customer == null)
                return ApiResponse<List<CustomerAppointmentForInvoiceDto>>
                    .ErrorResponse("Customer not found");

            var appointments = await _salesInvoiceRepository.GetCustomerAppointmentsAsync(customerId);

            var response = appointments.Select(a => new CustomerAppointmentForInvoiceDto
            {
                Id = a.Id,
                CustomerId = a.CustomerId,
                VehicleId = a.VehicleId,
                VehicleNumber = a.Vehicle?.VehicleNumber,
                ScheduledAt = a.ScheduledAt,
                Status = a.Status.ToString(),
                Notes = a.Notes
            }).ToList();

            return ApiResponse<List<CustomerAppointmentForInvoiceDto>>
                .SuccessResponse(response, "Customer appointments retrieved");
        }
    }
}