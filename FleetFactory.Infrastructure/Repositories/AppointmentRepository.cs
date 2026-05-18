using FleetFactory.Application.Interfaces.Repositories;
using FleetFactory.Domain.Entities;
using FleetFactory.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

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
    }
}