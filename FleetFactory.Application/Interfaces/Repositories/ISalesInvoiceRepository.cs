using FleetFactory.Domain.Entities;
using FleetFactory.Domain.Enums;
namespace FleetFactory.Application.Interfaces.Repositories
{
    public interface ISalesInvoiceRepository
    {
        Task<(List<SalesInvoice> Items, int TotalCount)> GetPagedAsync(int pageNumber, int pageSize);
        Task<SalesInvoice?> GetByIdAsync(Guid id);
        Task<bool> ExistsByInvoiceNoAsync(string invoiceNo);

        Task AddAsync(SalesInvoice invoice);
        void Update(SalesInvoice invoice);
        Task SaveChangesAsync();
        Task<(List<SalesInvoice> Items, int TotalCount)> SearchAsync(
            string? query,
            InvoiceStatus? status,
            SalesInvoiceMode? mode,
            int pageNumber,
            int pageSize);
        Task<List<Appointment>> GetCustomerAppointmentsAsync(Guid customerId);
        Task<Appointment?> GetAppointmentByIdAsync(Guid id);
        void UpdateAppointment(Appointment appointment);
    }
}