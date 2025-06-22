using EventManagementSystem.BLL.Services.Interfaces;
using EventManagementSystem.BLL.ViewModels.Notification;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EventManagementSystem.BLL.Services.Implementations
{
    public class NotificationService : INotificationService
    {
        public Task DeleteAllNotificationsByPersonIdAsync(int personId)
        {
            throw new NotImplementedException();
        }

        public Task DeleteNotificationAsync(int notificationId)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<NotificationListVM>> GetNotificationsByEventIdAsync(int eventId)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<NotificationListVM>> GetNotificationsByPersonIdAsync(int personId)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<NotificationListVM>> GetPendingNotificationsAsync(int batchSize = 100)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<NotificationListVM>> GetUnreadNotificationsByPersonIdAsync(int personId)
        {
            throw new NotImplementedException();
        }

        public Task SendNotificationAsync(int personId, string message, string title = "Notification", bool isRead = false, int? eventId = null)
        {
            throw new NotImplementedException();
        }
    }
}
