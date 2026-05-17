using FleetFactory.Application.Interfaces.Repositories;
using FleetFactory.Domain.Entities;
using FleetFactory.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore; 
using FleetFactory.Domain.Enums;
namespace FleetFactory.Infrastructure.Repositories
{
    public class PurchaseInvoiceRepository(AppDbContext _context) : IPurchaseInvoiceRepository
    {
        public async Task<(List<PurchaseInvoice> Items, int TotalCount)> GetPagedAsync(int pageNumber, int pageSize)
        {
            var query = _context.PurchaseInvoices
                .Include(p => p.Vendor)
                .Include(p => p.Items)
                    .ThenInclude(i => i.Part)
                .OrderByDescending(p => p.CreatedAt);

            var totalCount = await query.CountAsync();

            var items = await query
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            return (items, totalCount);
        }

        public async Task<PurchaseInvoice?> GetByIdAsync(Guid id)
        {
            return await _context.PurchaseInvoices
                .Include(p => p.Vendor)
                .Include(p => p.Items)
                    .ThenInclude(i => i.Part)
                .FirstOrDefaultAsync(p => p.Id == id);
        }

        public async Task<bool> ExistsByInvoiceNoAsync(string invoiceNo)
        {
            return await _context.PurchaseInvoices
                .AnyAsync(p => p.InvoiceNo == invoiceNo);
        }

        public async Task AddAsync(PurchaseInvoice invoice)
        {
            await _context.PurchaseInvoices.AddAsync(invoice);
        }

        public void Update(PurchaseInvoice invoice)
        {
            _context.PurchaseInvoices.Update(invoice); //cant use executeupdateasync because we need to update the related items as well, and executeupdateasync does not support updating related entities.
        }

        public async Task SaveChangesAsync()
        {
            await _context.SaveChangesAsync();
        }

        public async Task<(List<PurchaseInvoice> Items, int TotalCount)> SearchAsync(
            string? query,
            InvoiceStatus? status,
            int pageNumber,
            int pageSize)
        {
            var invoicesQuery = _context.PurchaseInvoices
                .Include(p => p.Vendor)
                .Include(p => p.Items)
                    .ThenInclude(i => i.Part)
                .AsQueryable();

            if (!string.IsNullOrWhiteSpace(query))
            {
                var keyword = query.ToLower();

                invoicesQuery = invoicesQuery.Where(p =>
                    p.InvoiceNo.ToLower().Contains(keyword) ||
                    p.Vendor.Name.ToLower().Contains(keyword)
                );
            }

            if (status.HasValue)
            {
                invoicesQuery = invoicesQuery.Where(p => p.Status == status.Value);
            }

            invoicesQuery = invoicesQuery.OrderByDescending(p => p.CreatedAt);

            var totalCount = await invoicesQuery.CountAsync();

            var items = await invoicesQuery
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            return (items, totalCount);
        }
    }
}