using FleetFactory.Domain.Entities;

namespace FleetFactory.Application.Interfaces.Repositories
{
    public interface IAppointmentRepository
    {
        Task<(List<Appointment> Appointments, int TotalCount)> GetPagedAsync(
            int pageNumber,
            int pageSize);

        Task<Appointment?> GetByIdAsync(Guid id);

        Task AddAsync(Appointment appointment);

        void Update(Appointment appointment);

        Task SaveChangesAsync();
    }
}