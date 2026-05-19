using FleetFactory.Application.Interfaces.Services;
using FleetFactory.Domain.Entities;
using FleetFactory.Infrastructure.Helpers;
using FleetFactory.Infrastructure.Persistence;

namespace FleetFactory.Infrastructure.Services
{
    public class NotificationService(
        AppDbContext _context
    ) : INotificationService
    {
        public async Task CreateAsync(
            string? userId,
            string type,
            string title,
            string message,
            Guid? referenceId = null)
        {
            var notification = new Notification
            {
                UserId = userId,
                Type = type,
                Title = title,
                Message = message,
                ReferenceId = referenceId,
                IsRead = false,
                CreatedAt = DateTimeHelper.UtcNow
            };

            await _context.Notifications.AddAsync(notification);
            await _context.SaveChangesAsync();
        }
    }
}