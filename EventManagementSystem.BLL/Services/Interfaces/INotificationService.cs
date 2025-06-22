using EventManagementSystem.BLL.ViewModels.Notification;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EventManagementSystem.BLL.Services.Interfaces
{
    public interface INotificationService
    {
        Task SendNotificationAsync(int personId, string message, string title = "Notification", bool isRead = false, int? eventId = null);
        Task<IEnumerable<NotificationListVM>> GetNotificationsByPersonIdAsync(int personId);
        Task<IEnumerable<NotificationListVM>> GetUnreadNotificationsByPersonIdAsync(int personId);
        Task DeleteNotificationAsync(int notificationId);
        Task DeleteAllNotificationsByPersonIdAsync(int personId);
        Task<IEnumerable<NotificationListVM>> GetPendingNotificationsAsync(int batchSize = 100);
        Task<IEnumerable<NotificationListVM>> GetNotificationsByEventIdAsync(int eventId);

    }
}
