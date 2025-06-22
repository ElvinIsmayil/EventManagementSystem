using EventManagementSystem.DAL.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EventManagementSystem.BLL.ViewModels.Notification
{
    public record NotificationDetailsVM
    {
        public int Id { get; set; } // From BaseEntity
        public string Message { get; set; } = string.Empty;
        public DateTime SentAt { get; set; }

        public int? EventId { get; set; }
        public string? EventTitle { get; set; } // Navigation property related field for display
        public int PersonId { get; set; }
        public string PersonName { get; set; } = string.Empty; // Navigation property related field for display

        public NotificationType Type { get; set; }
        public bool IsRead { get; set; }

        public string RecipientEmail { get; set; } = string.Empty;
        public NotificationStatus? Status { get; set; }
        public string? ErrorMessage { get; set; }
        public DateTime? ScheduledAt { get; set; }
    }
}
