using EventManagementSystem.DAL.Entities.Common;
using EventManagementSystem.DAL.Enums;

namespace EventManagementSystem.DAL.Entities
{
    public class Notification : BaseEntity
    {
        public string Message { get; set; } = string.Empty;
        public DateTime SentAt { get; set; } = DateTime.UtcNow;

        public int? EventId { get; set; }
        public int PersonId { get; set; }

        public Event? Event { get; set; }
        public Person Person { get; set; } = null!;

        public NotificationType Type { get; set; }
        public bool IsRead { get; set; } = false;

        public string RecipientEmail { get; set; } = string.Empty;
        public NotificationStatus? Status { get; set; } = NotificationStatus.Pending;
        public string? ErrorMessage { get; set; } = null;
        public DateTime? ScheduledAt { get; set; }

    }


}
