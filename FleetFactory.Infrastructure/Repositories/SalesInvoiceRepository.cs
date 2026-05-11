using FleetFactory.Application.Interfaces.Repositories;
using FleetFactory.Domain.Entities;
using FleetFactory.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace FleetFactory.Infrastructure.Repositories
{
    public class SalesInvoiceRepository(AppDbContext _context) : ISalesInvoiceRepository
    {
        public async Task<(List<SalesInvoice> Items, int TotalCount)> GetPagedAsync(int pageNumber, int pageSize)
        {
            var query = _context.SalesInvoices
                .Include(s => s.Customer)
                .Include(s => s.Vehicle)
                .Include(s => s.Appointment)
                .Include(s => s.Items)
                    .ThenInclude(i => i.Part)
                .OrderByDescending(s => s.CreatedAt);

            var totalCount = await query.CountAsync();

            var items = await query
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            return (items, totalCount);
        }

        public async Task<SalesInvoice?> GetByIdAsync(Guid id)
        {
            return await _context.SalesInvoices
                .Include(s => s.Customer)
                .Include(s => s.Vehicle)
                .Include(s => s.Appointment)
                .Include(s => s.Items)
                    .ThenInclude(i => i.Part)
                .FirstOrDefaultAsync(s => s.Id == id);
        }

        public async Task<bool> ExistsByInvoiceNoAsync(string invoiceNo)
        {
            return await _context.SalesInvoices
                .AnyAsync(s => s.InvoiceNo == invoiceNo);
        }

        public async Task AddAsync(SalesInvoice invoice)
        {
            await _context.SalesInvoices.AddAsync(invoice);
        }

        public void Update(SalesInvoice invoice)
        {
            _context.SalesInvoices.Update(invoice);
        }

        public async Task SaveChangesAsync()
        {
            await _context.SaveChangesAsync();
        }
    }
}