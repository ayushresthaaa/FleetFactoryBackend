using FleetFactory.Application.Interfaces.Repositories;
using FleetFactory.Domain.Entities;
using FleetFactory.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace FleetFactory.Infrastructure.Repositories
{
    public class CustomerSideRepository(AppDbContext _context) : ICustomerSideRepository
    {
        public async Task<List<SalesInvoice>> GetPurchaseHistoryAsync(Guid customerId)
        {
            return await _context.SalesInvoices
                .Where(s => s.CustomerId == customerId)
                .OrderByDescending(s => s.CreatedAt)
                .ToListAsync();
        }
    }
}