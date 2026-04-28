using FleetFactory.Application.Features.PurchaseInvoices.DTOs;
using FleetFactory.Application.Interfaces.Repositories;
using FleetFactory.Application.Interfaces.Services;
using FleetFactory.Domain.Entities;
using FleetFactory.Domain.Enums;
using FleetFactory.Shared.Results;
using FleetFactory.Infrastructure.Helpers;
namespace FleetFactory.Application.Features.PurchaseInvoices.Services
{
    public class PurchaseInvoiceService(IPurchaseInvoiceRepository _purchaseInvoiceRepository, IPartRepository _partRepository): IPurchaseInvoiceService { 
        
        public async Task<ApiResponse<PagedResult<PurchaseInvoiceResponseDto>>> GetAllAsync(int pageNumber, int pageSize)
        {
            pageNumber = pageNumber < 1 ? 1 : pageNumber; 
            pageSize = pageSize < 1 ? 10 : pageSize;

            var (invoices, totalCount) = await _purchaseInvoiceRepository.GetPagedAsync(pageNumber, pageSize);

            var response = invoices.Select(invoice => new PurchaseInvoiceResponseDto
            {
                Id = invoice.Id,
                InvoiceNo = invoice.InvoiceNo,
                VendorId = invoice.VendorId,
                VendorName = invoice.Vendor?.Name ?? "",
                TotalAmount = invoice.TotalAmount,
                Status = invoice.Status.ToString(),
                CreatedAt = invoice.CreatedAt,
                Items = invoice.Items.Select(i => new PurchaseInvoiceItemResponseDto
                {
                    Id = i.Id,
                    PartId = i.PartId,
                    PartName = i.Part?.Name ?? "",
                    Quantity = i.Quantity,
                    UnitCost = i.UnitCost,
                    Subtotal = i.Quantity * i.UnitCost
                }).ToList()
            }).ToList();

            var pagedResult = PagedResult<PurchaseInvoiceResponseDto>.Create(
                response,
                pageNumber,
                pageSize,
                totalCount
            );

            return ApiResponse<PagedResult<PurchaseInvoiceResponseDto>>
                .SuccessResponse(pagedResult, "Purchase invoices retrieved successfully");
        }
        
        public async Task<ApiResponse<PurchaseInvoiceResponseDto>> GetByIdAsync(Guid id)
        {
            var invoice = await _purchaseInvoiceRepository.GetByIdAsync(id);

            if (invoice == null)
                return ApiResponse<PurchaseInvoiceResponseDto>.ErrorResponse("Purchase invoice not found");

            var response = new PurchaseInvoiceResponseDto
            {
                Id = invoice.Id,
                InvoiceNo = invoice.InvoiceNo,
                VendorId = invoice.VendorId,
                VendorName = invoice.Vendor?.Name ?? "",
                TotalAmount = invoice.TotalAmount,
                Status = invoice.Status.ToString(),
                CreatedAt = invoice.CreatedAt,
                Items = invoice.Items.Select(i => new PurchaseInvoiceItemResponseDto
                {
                    Id = i.Id,
                    PartId = i.PartId,
                    PartName = i.Part?.Name ?? "",
                    Quantity = i.Quantity,
                    UnitCost = i.UnitCost,
                    Subtotal = i.Quantity * i.UnitCost
                }).ToList()
            };

            return ApiResponse<PurchaseInvoiceResponseDto>
                .SuccessResponse(response, "Purchase invoice fetched successfully");
        }

        public async Task<ApiResponse<PurchaseInvoiceResponseDto>> CreateAsync(
            CreatePurchaseInvoiceRequestDto request,
            string createdById)
        {
            if (request.Items == null || request.Items.Count == 0)
                return ApiResponse<PurchaseInvoiceResponseDto>.ErrorResponse("Invoice must contain at least one item");

            var invoiceExists = await _purchaseInvoiceRepository.ExistsByInvoiceNoAsync(request.InvoiceNo); //checks if invoice exists with same no

            if (invoiceExists)
                return ApiResponse<PurchaseInvoiceResponseDto>.ErrorResponse("Invoice number already exists");

            var invoice = new PurchaseInvoice
            {
                VendorId = request.VendorId,
                InvoiceNo = request.InvoiceNo.Trim(),
                CreatedById = createdById,
                Status = InvoiceStatus.Pending,
                CreatedAt = DateTimeHelper.UtcNow,
                UpdatedAt = DateTimeHelper.UtcNow
            };

            decimal totalAmount = 0;

            foreach (var item in request.Items)
            {
                var part = await _partRepository.GetByIdAsync(item.PartId);

                if (part == null)
                    return ApiResponse<PurchaseInvoiceResponseDto>.ErrorResponse($"Part not found: {item.PartId}");

                totalAmount += item.Quantity * item.UnitCost;

                invoice.Items.Add(new PurchaseInvoiceItem
                {
                    PartId = item.PartId,
                    Quantity = item.Quantity,
                    UnitCost = item.UnitCost
                });

                part.StockQty += item.Quantity;
                part.UpdatedAt = DateTimeHelper.UtcNow;

                part.StockMovements.Add(new StockMovement
                {
                    PartId = part.Id,
                    MovementType = StockMovementType.Purchase,
                    Quantity = item.Quantity,
                    ReferenceId = invoice.Id,
                    Note = $"Stock added from purchase invoice {request.InvoiceNo}",
                    CreatedById = createdById,
                    CreatedAt = DateTimeHelper.UtcNow
                });

                _partRepository.Update(part);
            }

            invoice.TotalAmount = totalAmount;

            await _purchaseInvoiceRepository.AddAsync(invoice);
            await _purchaseInvoiceRepository.SaveChangesAsync();

            var response = new PurchaseInvoiceResponseDto
            {
                Id = invoice.Id,
                InvoiceNo = invoice.InvoiceNo,
                VendorId = invoice.VendorId,
                VendorName = invoice.Vendor?.Name ?? "",
                TotalAmount = invoice.TotalAmount,
                Status = invoice.Status.ToString(),
                CreatedAt = invoice.CreatedAt,
                Items = invoice.Items.Select(i => new PurchaseInvoiceItemResponseDto
                {
                    Id = i.Id,
                    PartId = i.PartId,
                    PartName = i.Part?.Name ?? "",
                    Quantity = i.Quantity,
                    UnitCost = i.UnitCost,
                    Subtotal = i.Quantity * i.UnitCost
                }).ToList()
            };

            return ApiResponse<PurchaseInvoiceResponseDto>
                .SuccessResponse(response, "Purchase invoice created successfully");
        }

           public async Task<ApiResponse<PurchaseInvoiceResponseDto>> StatusUpdateAsync(Guid id)
            {
                var invoice = await _purchaseInvoiceRepository.GetByIdAsync(id);

                if (invoice == null)
                    return ApiResponse<PurchaseInvoiceResponseDto>.ErrorResponse("Purchase invoice not found");

                invoice.Status = InvoiceStatus.Paid;
                invoice.PaidAt = DateTimeHelper.UtcNow;
                invoice.UpdatedAt = DateTimeHelper.UtcNow;

                _purchaseInvoiceRepository.Update(invoice);
                await _purchaseInvoiceRepository.SaveChangesAsync();

                var response = new PurchaseInvoiceResponseDto
                {
                    Id = invoice.Id,
                    InvoiceNo = invoice.InvoiceNo,
                    VendorId = invoice.VendorId,
                    VendorName = invoice.Vendor?.Name ?? "",
                    TotalAmount = invoice.TotalAmount,
                    Status = invoice.Status.ToString(),
                    CreatedAt = invoice.CreatedAt,
                    Items = invoice.Items.Select(i => new PurchaseInvoiceItemResponseDto
                    {
                        Id = i.Id,
                        PartId = i.PartId,
                        PartName = i.Part?.Name ?? "",
                        Quantity = i.Quantity,
                        UnitCost = i.UnitCost,
                        Subtotal = i.Quantity * i.UnitCost
                    }).ToList()
                };

                return ApiResponse<PurchaseInvoiceResponseDto>
                    .SuccessResponse(response, "Purchase invoice status updated successfully");
            }

    }
}