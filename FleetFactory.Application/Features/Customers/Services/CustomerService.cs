using FleetFactory.Application.Features.Customers.DTOs;
using FleetFactory.Application.Interfaces.Repositories;
using FleetFactory.Application.Interfaces.Services;
using FleetFactory.Domain.Entities;
using FleetFactory.Infrastructure.Helpers;
using FleetFactory.Infrastructure.Identity;
using FleetFactory.Shared.Results;
using Microsoft.AspNetCore.Identity;

namespace FleetFactory.Application.Features.Customers.Services
{
    public class CustomerService(
        ICustomerRepository _customerRepository,
        UserManager<ApplicationUser> _userManager
    ) : ICustomerService
    {
        public async Task<ApiResponse<CustomerResponseDto>> CreateAsync(CreateCustomerWithVehicleRequestDto request)
        {
            // check duplicate vehicle
            var vehicleExists = await _customerRepository.VehicleNumberExistsAsync(request.VehicleNumber);
            if (vehicleExists)
                return ApiResponse<CustomerResponseDto>.ErrorResponse("Vehicle number already exists");

            // create identity user
            var user = new ApplicationUser
            {
                UserName = request.Email,
                Email = request.Email,
                FirstName = request.FullName,
                LastName = "",
                IsActive = true
            };

            var password = "Customer@123"; // temporary default password will be changed by customer later

            var userResult = await _userManager.CreateAsync(user, password);

            if (!userResult.Succeeded)
            {
                var errors = string.Join(", ", userResult.Errors.Select(e => e.Description)); //we can use toString 

                return ApiResponse<CustomerResponseDto>.ErrorResponse(errors);
            }

            // create customer profile
            var customer = new CustomerProfile
            {
                UserId = user.Id,
                FullName = request.FullName,
                Phone = request.Phone,
                Address = request.Address,
                CreditBalance = 0,
                CreatedAt = DateTimeHelper.UtcNow,
                UpdatedAt = DateTimeHelper.UtcNow
            };

            // create vehicle
            customer.Vehicles.Add(new Vehicle
            {
                VehicleNumber = request.VehicleNumber,
                Make = request.Make,
                Model = request.Model,
                Year = request.Year,
                CreatedAt = DateTimeHelper.UtcNow,
                UpdatedAt = DateTimeHelper.UtcNow
            });

            await _customerRepository.AddAsync(customer);
            await _customerRepository.SaveChangesAsync();

            return await GetByIdAsync(customer.Id);
        }

        public async Task<ApiResponse<PagedResult<CustomerResponseDto>>> GetAllAsync(int pageNumber, int pageSize)
        {
            pageNumber = pageNumber < 1 ? 1 : pageNumber;
            pageSize = pageSize < 1 ? 10 : pageSize;

            var (customers, totalCount) = await _customerRepository.GetPagedAsync(pageNumber, pageSize);

            var response = customers.Select(c => new CustomerResponseDto
            {
                Id = c.Id,
                UserId = c.UserId,
                Email = c.User?.Email ?? "",
                FullName = c.FullName,
                Phone = c.Phone,
                Address = c.Address,
                CreditBalance = c.CreditBalance,
                CreatedAt = c.CreatedAt,
                Vehicles = c.Vehicles.Select(v => new VehicleResponseDto
                {
                    Id = v.Id,
                    VehicleNumber = v.VehicleNumber,
                    Make = v.Make,
                    Model = v.Model,
                    Year = v.Year
                }).ToList()
            }).ToList();

            var paged = PagedResult<CustomerResponseDto>.Create(
                response,
                pageNumber,
                pageSize,
                totalCount
            );

            return ApiResponse<PagedResult<CustomerResponseDto>>
                .SuccessResponse(paged, "Customers retrieved successfully");
        }

        public async Task<ApiResponse<CustomerResponseDto>> GetByIdAsync(Guid id)
        {
            var customer = await _customerRepository.GetByIdAsync(id);

            if (customer == null)
                return ApiResponse<CustomerResponseDto>.ErrorResponse("Customer not found");

            var response = new CustomerResponseDto
            {
                Id = customer.Id,
                UserId = customer.UserId,
                Email = "", // optional later
                FullName = customer.FullName,
                Phone = customer.Phone,
                Address = customer.Address,
                CreditBalance = customer.CreditBalance,
                CreatedAt = customer.CreatedAt,
                Vehicles = customer.Vehicles.Select(v => new VehicleResponseDto
                {
                    Id = v.Id,
                    VehicleNumber = v.VehicleNumber,
                    Make = v.Make,
                    Model = v.Model,
                    Year = v.Year
                }).ToList()
            };

            return ApiResponse<CustomerResponseDto>
                .SuccessResponse(response, "Customer fetched successfully");
        }
        public async Task<ApiResponse<CustomerResponseDto>> AddVehicleAsync(Guid customerId, AddVehicleRequestDto request)
        {
            var customer = await _customerRepository.GetByIdAsync(customerId);

            if (customer == null)
                return ApiResponse<CustomerResponseDto>.ErrorResponse("Customer not found");

            var vehicleExists = await _customerRepository.VehicleNumberExistsAsync(request.VehicleNumber);

            if (vehicleExists)
                return ApiResponse<CustomerResponseDto>.ErrorResponse("Vehicle number already exists");

            await _customerRepository.AddVehicleAsync(new Vehicle
            {
                CustomerId = customer.Id,
                VehicleNumber = request.VehicleNumber.Trim(),
                Make = request.Make,
                Model = request.Model,
                Year = request.Year,
                CreatedAt = DateTimeHelper.UtcNow,
                UpdatedAt = DateTimeHelper.UtcNow
            });

            customer.UpdatedAt = DateTimeHelper.UtcNow;
            _customerRepository.Update(customer);

            await _customerRepository.SaveChangesAsync();

            return await GetByIdAsync(customerId);
        }


        //rachina part
        public async Task<ApiResponse<CustomerHistoryResponseDTO>> GetCustomerHistoryAsync(Guid customerId)
        {
            var customer = await _customerRepository.GetWithHistoryAsync(customerId);

            if (customer == null)
                return ApiResponse<CustomerHistoryResponseDTO>.ErrorResponse("Customer not found");

            var response = new CustomerHistoryResponseDTO
            {
                CustomerId = customer.Id,
                UserId = customer.UserId,
                FullName = customer.FullName,
                Phone = customer.Phone,
                Address = customer.Address,
                CreditBalance = customer.CreditBalance,

                Vehicles = customer.Vehicles.Select(v => new VehicleResponseDto
                {
                    Id = v.Id,
                    VehicleNumber = v.VehicleNumber,
                    Make = v.Make,
                    Model = v.Model,
                    Year = v.Year
                }).ToList(),

                PurchaseHistory = customer.SalesInvoices.Select(s => new CustomerSalesInvoiceHistoryDTO
                {
                    SalesInvoiceId = s.Id,
                    InvoiceNo = s.InvoiceNo,
                    Status = s.Status,
                    Subtotal = s.Subtotal,
                    DiscountPct = s.DiscountPct,
                    TotalAmount = s.TotalAmount,
                    CreatedAt = s.CreatedAt
                }).ToList()
            };

            return ApiResponse<CustomerHistoryResponseDTO>
                .SuccessResponse(response, "Customer history retrieved successfully");
        }

        //rabison part 
        public async Task<ApiResponse<List<CustomerSearchResponseDto>>> SearchAsync(string query)
        {
            if (string.IsNullOrWhiteSpace(query))
                return ApiResponse<List<CustomerSearchResponseDto>>
                    .ErrorResponse("Search query is required");

            var customers = await _customerRepository.SearchAsync(query);

            var response = customers.Select(c => new CustomerSearchResponseDto
            {
                CustomerId = c.Id,
                UserId = c.UserId,
                FullName = c.FullName,
                Phone = c.Phone,
                Address = c.Address,
                Vehicles = c.Vehicles.Select(v => new VehicleResponseDto
                {
                    Id = v.Id,
                    VehicleNumber = v.VehicleNumber,
                    Make = v.Make,
                    Model = v.Model,
                    Year = v.Year
                }).ToList()
            }).ToList();

            return ApiResponse<List<CustomerSearchResponseDto>>
                .SuccessResponse(response, "Customers searched successfully");
        }
    }

}