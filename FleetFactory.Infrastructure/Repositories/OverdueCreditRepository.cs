using FleetFactory.Application.Interfaces.Repositories;
using FleetFactory.Domain.Entities;
using FleetFactory.Domain.Enums;
using FleetFactory.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace FleetFactory.Infrastructure.Repositories
{
    public class OverdueCreditRepository(AppDbContext _context) : IOverdueCreditRepository
    {
       public async Task<List<CustomerProfile>> GetOverdueCreditCustomersAsync(DateTime thresholdDate)
        {
            var customerIds = await _context.SalesInvoices
                .Where(s =>
                    s.PaymentMethod == PaymentMethod.Credit &&
                    s.Status != InvoiceStatus.Paid &&
                    s.Status != InvoiceStatus.Cancelled &&
                    s.DueDate != null &&
                    s.DueDate <= DateTime.UtcNow
                )
                .Select(s => s.CustomerId)
                .Distinct()
                .ToListAsync();

            var customers = await _context.CustomerProfiles
                .Include(c => c.User)
                .Where(c => customerIds.Contains(c.Id))
                .ToListAsync();

            foreach (var customer in customers)
            {
                customer.CreditBalance = await _context.SalesInvoices
                    .Where(s =>
                        s.CustomerId == customer.Id &&
                        s.PaymentMethod == PaymentMethod.Credit &&
                        s.Status != InvoiceStatus.Paid &&
                        s.Status != InvoiceStatus.Cancelled &&
                        s.DueDate != null &&
                        s.DueDate <= DateTime.UtcNow
                    )
                    .SumAsync(s => s.TotalAmount);
            }

            return customers
                .OrderByDescending(c => c.CreditBalance)
                .ToList();
        }
       public async Task<CustomerProfile?> GetOverdueCreditCustomerByIdAsync(
            Guid customerId,
            DateTime thresholdDate
        )
        {
            var hasCredit = await _context.SalesInvoices
                .AnyAsync(s =>
                    s.CustomerId == customerId &&
                    s.PaymentMethod == PaymentMethod.Credit &&
                    s.Status != InvoiceStatus.Paid &&
                    s.Status != InvoiceStatus.Cancelled &&
                    s.DueDate != null &&
                    s.DueDate <= DateTime.UtcNow
                );

            if (!hasCredit)
                return null;

            var customer = await _context.CustomerProfiles
                .Include(c => c.User)
                .FirstOrDefaultAsync(c => c.Id == customerId);

            if (customer == null)
                return null;

            customer.CreditBalance = await _context.SalesInvoices
                .Where(s =>
                    s.CustomerId == customer.Id &&
                    s.PaymentMethod == PaymentMethod.Credit &&
                    s.Status != InvoiceStatus.Paid &&
                    s.Status != InvoiceStatus.Cancelled &&
                    s.DueDate != null &&
                    s.DueDate <= DateTime.UtcNow
                )
                .SumAsync(s => s.TotalAmount);

                      return customer;
        }

        public async Task AddNotificationAsync(Notification notification)
        {
            await _context.Notifications.AddAsync(notification);
        }

        public async Task SaveChangesAsync()
        {
            await _context.SaveChangesAsync();
        }
    }
}