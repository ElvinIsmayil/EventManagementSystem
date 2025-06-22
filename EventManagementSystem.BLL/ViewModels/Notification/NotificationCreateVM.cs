using EventManagementSystem.DAL.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EventManagementSystem.BLL.ViewModels.Notification
{
    public record NotificationCreateVM
    {
        [Required(ErrorMessage = "Message is required.")]
        [StringLength(1000, ErrorMessage = "Message cannot exceed 1000 characters.")]
        public string Message { get; set; } = string.Empty;

        // SentAt is usually set by the service or DB on creation, so no [Required]
        // public DateTime SentAt { get; set; } // Not typically provided by client

        public int? EventId { get; set; }

        [Required(ErrorMessage = "Person ID is required.")]
        public int PersonId { get; set; }

        [Required(ErrorMessage = "Notification Type is required.")]
        public NotificationType Type { get; set; }

        // IsRead is typically false on creation, set by service
        // public bool IsRead { get; set; } = false;

        [Required(ErrorMessage = "Recipient email is required.")]
        [EmailAddress(ErrorMessage = "Invalid email format.")]
        public string RecipientEmail { get; set; } = string.Empty;

        // Status is often set by the service (e.g., Pending, then Sent/Failed)
        public NotificationStatus? Status { get; set; } = NotificationStatus.Pending;

        // ErrorMessage is for internal tracking, not typically set on creation
        // public string? ErrorMessage { get; set; } = null;

        public DateTime? ScheduledAt { get; set; } // Can be provided for scheduled notifications
    }
}
