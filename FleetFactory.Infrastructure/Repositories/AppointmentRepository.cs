using FleetFactory.Application.Interfaces.Repositories;
using FleetFactory.Domain.Entities;
using FleetFactory.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using FleetFactory.Domain.Enums;
namespace FleetFactory.Infrastructure.Repositories
{
    public class AppointmentRepository(AppDbContext _context)
        : IAppointmentRepository
    {
        public async Task<(List<Appointment> Appointments, int TotalCount)>
            GetPagedAsync(int pageNumber, int pageSize)
        {
            var query = _context.Appointments
                .Include(a => a.Customer)
                .Include(a => a.Vehicle)
                .OrderByDescending(a => a.ScheduledAt);

            var totalCount = await query.CountAsync();

            var appointments = await query
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            return (appointments, totalCount);
        }

        public async Task<Appointment?> GetByIdAsync(Guid id)
        {
            return await _context.Appointments
                .Include(a => a.Customer)
                .Include(a => a.Vehicle)
                .FirstOrDefaultAsync(a => a.Id == id);
        }

        public async Task AddAsync(Appointment appointment)
        {
            await _context.Appointments.AddAsync(appointment);
        }

        public void Update(Appointment appointment)
        {
            _context.Appointments.Update(appointment);
        }

        public async Task SaveChangesAsync()
        {
            await _context.SaveChangesAsync();
        }
        public async Task<(List<Appointment> Appointments, int TotalCount)> SearchAsync(
            string? query,
            AppointmentStatus? status,
            int pageNumber,
            int pageSize)
        {
            var appointmentsQuery = _context.Appointments
                .Include(a => a.Customer)
                .Include(a => a.Vehicle)
                .AsQueryable();

            if (!string.IsNullOrWhiteSpace(query))
            {
                query = query.Trim().ToLower();

                appointmentsQuery = appointmentsQuery.Where(a =>
                    a.Customer.FullName.ToLower().Contains(query) ||
                    (a.Vehicle != null &&
                    a.Vehicle.VehicleNumber.ToLower().Contains(query))
                );
            }

            if (status.HasValue)
            {
                appointmentsQuery = appointmentsQuery
                    .Where(a => a.Status == status.Value);
            }

            appointmentsQuery = appointmentsQuery
                .OrderByDescending(a => a.ScheduledAt);

            var totalCount = await appointmentsQuery.CountAsync();

            var appointments = await appointmentsQuery
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            return (appointments, totalCount);
        }
    }
}