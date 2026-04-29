using FleetFactory.Application.Interfaces.Repositories;
using FleetFactory.Domain.Entities;
using FleetFactory.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace FleetFactory.Infrastructure.Repositories
{
    public class ReportRepository(AppDbContext _context) : IReportRepository
    {
        public async Task<List<SalesInvoice>> GetSalesInvoicesByDateRangeAsync(DateTime fromDate, DateTime toDate)
        {
            return await _context.SalesInvoices
                .Where(s => s.CreatedAt >= fromDate && s.CreatedAt <= toDate)
                .ToListAsync();
        }

        public async Task<List<PurchaseInvoice>> GetPurchaseInvoicesByDateRangeAsync(DateTime fromDate, DateTime toDate)
        {
            return await _context.PurchaseInvoices
                .Where(p => p.CreatedAt >= fromDate && p.CreatedAt <= toDate)
                .ToListAsync();
        }
    }
}