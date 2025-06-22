using EventManagementSystem.DAL.Data;
using EventManagementSystem.DAL.Entities;
using EventManagementSystem.DAL.Enums;
using EventManagementSystem.DAL.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EventManagementSystem.DAL.Repositories.Implementations
{
    public class NotificationRepository : GenericRepository<Notification>, INotificationRepository
    {
        public NotificationRepository(EventManagementSystemDbContext context) : base(context)
        {
        }
        public async Task<IEnumerable<Notification>> GetByPersonIdAsync(int personId)
        {
            return await _context.Notifications
                .Where(n => n.PersonId == personId)
                .ToListAsync();
        }
        public async Task<IEnumerable<Notification>> GetUnreadByPersonIdAsync(int personId)
        {
            return await _context.Notifications
                .Where(n => n.PersonId == personId && !n.IsRead)
                .ToListAsync();
        }
        public async Task<IEnumerable<Notification>> GetPendingNotificationsAsync(int batchSize = 100)
        {
            return await _context.Notifications
                .Where(n => n.Status == NotificationStatus.Pending)
                .Take(batchSize)
                .ToListAsync();
        }
        public async Task<IEnumerable<Notification>> GetByEventIdAsync(int eventId)
        {
            return await _context.Notifications
                .Where(n => n.EventId == eventId)
                .ToListAsync();
        }
    }
}
