using FleetFactory.Application.Interfaces.Repositories;
using FleetFactory.Domain.Entities;
using FleetFactory.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace FleetFactory.Infrastructure.Repositories;

public class SalesInvoiceEmailRepository : ISalesInvoiceEmailRepository
{
    private readonly AppDbContext _context;

    public SalesInvoiceEmailRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<SalesInvoice?> GetInvoiceWithCustomerAsync(Guid salesInvoiceId)
    {
        return await _context.SalesInvoices
            .Include(x => x.CustomerProfile)
            .Include(x => x.SalesInvoiceItems)
            .FirstOrDefaultAsync(x => x.Id == salesInvoiceId);
    }
}