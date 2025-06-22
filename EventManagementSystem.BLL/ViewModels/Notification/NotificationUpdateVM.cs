using EventManagementSystem.DAL.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EventManagementSystem.BLL.ViewModels.Notification
{
    public record NotificationUpdateVM
    {
        [Required(ErrorMessage = "Notification ID is required for update.")]
        public int Id { get; set; } // To identify which notification to update

        // Properties that are commonly updated after creation
        public bool? IsRead { get; set; } // To mark as read/unread
        public NotificationStatus? Status { get; set; } // To update delivery status (e.g., from Pending to Sent, Failed, etc.)
        public string? ErrorMessage { get; set; } // To log sending failures

        // Less common to update these, but included for completeness if needed:
        public string? Message { get; set; } // If notification message can be edited post-creation
        public DateTime? ScheduledAt { get; set; } // If a scheduled notification's time needs adjustment
        public string? RecipientEmail { get; set; } // If recipient e
    }
}
