using FleetFactory.Application.Interfaces.Repositories;
using FleetFactory.Domain.Entities;
using FleetFactory.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using FleetFactory.Domain.Enums;
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

        
        public async Task<(List<SalesInvoice> Items, int TotalCount)> SearchAsync(
            string? query,
            InvoiceStatus? status,
            SalesInvoiceMode? invoiceMode,
            int pageNumber,
            int pageSize)
        {
            var invoices = _context.SalesInvoices
                .Include(s => s.Customer)
                .Include(s => s.Vehicle)
                .Include(s => s.Appointment)
                .Include(s => s.Items)
                    .ThenInclude(i => i.Part)
                .AsQueryable();

            if (!string.IsNullOrWhiteSpace(query))
            {
                var keyword = query.Trim().ToLower();

                invoices = invoices.Where(s =>
                    s.InvoiceNo.ToLower().Contains(keyword) ||
                    s.Id.ToString().ToLower().Contains(keyword) ||
                    s.Customer.FullName.ToLower().Contains(keyword) ||
                    (s.Customer.Phone != null && s.Customer.Phone.ToLower().Contains(keyword)) ||
                    (s.Vehicle != null && s.Vehicle.VehicleNumber.ToLower().Contains(keyword))
                );
            }

            if (status.HasValue)
            {
                invoices = invoices.Where(s => s.Status == status.Value);
            }

            if (invoiceMode == SalesInvoiceMode.Appointment)
            {
                invoices = invoices.Where(s => s.AppointmentId != null);
            }
            else if (invoiceMode == SalesInvoiceMode.Direct)
            {
                invoices = invoices.Where(s => s.AppointmentId == null);
            }

            invoices = invoices.OrderByDescending(s => s.CreatedAt);

            var totalCount = await invoices.CountAsync();

            var items = await invoices
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            return (items, totalCount);
        }

        //this gets appointment for a customer to be used in invoice creation
        public async Task<List<Appointment>> GetCustomerAppointmentsAsync(Guid customerId)
        {
            return await _context.Appointments
                .Include(a => a.Vehicle)
                .Where(a => a.CustomerId == customerId)
                .OrderByDescending(a => a.ScheduledAt)
                .ToListAsync();
        }

        //this gets appointment by id for invoice creation to mark it as completed
        public async Task<Appointment?> GetAppointmentByIdAsync(Guid id)
        {
            return await _context.Appointments
                .Include(a => a.Vehicle)
                .FirstOrDefaultAsync(a => a.Id == id);
        }

        //this marks appointment as completed when linked to an invoice
        public void UpdateAppointment(Appointment appointment)
        {
            _context.Appointments.Update(appointment);
        }
    }
}