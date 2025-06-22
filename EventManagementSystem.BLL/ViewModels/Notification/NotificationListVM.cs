using EventManagementSystem.DAL.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EventManagementSystem.BLL.ViewModels.Notification
{
    public record NotificationListVM
    {
        public int Id { get; set; } // From BaseEntity
        public string Message { get; set; } = string.Empty;
        public DateTime SentAt { get; set; }
        public NotificationType Type { get; set; }
        public bool IsRead { get; set; }
        public int PersonId { get; set; }
        public string RecipientEmail { get; set; } = string.Empty;
        public NotificationStatus? Status { get; set; }
    }
}
