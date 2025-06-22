using EventManagementSystem.DAL.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EventManagementSystem.DAL.Repositories.Interfaces
{
    public interface INotificationRepository : IGenericRepository<Notification>
    { 
        Task<IEnumerable<Notification>> GetByPersonIdAsync(int personId);

        Task<IEnumerable<Notification>> GetUnreadByPersonIdAsync(int personId);

        Task<IEnumerable<Notification>> GetPendingNotificationsAsync(int batchSize = 100);

        Task<IEnumerable<Notification>> GetByEventIdAsync(int eventId);

        
    }
}
