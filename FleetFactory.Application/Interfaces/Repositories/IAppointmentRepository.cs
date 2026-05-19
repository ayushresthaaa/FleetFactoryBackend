using FleetFactory.Domain.Entities;
using FleetFactory.Domain.Enums;
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
        Task<(List<Appointment> Appointments, int TotalCount)> SearchAsync(
            string? query,
            AppointmentStatus? status,
            int pageNumber,
            int pageSize);

        Task<int> CountActiveAppointmentsByDateAsync(DateTime date);
    }
}