using EventManagementSystem.BLL.ViewModels.Feedback;
using EventManagementSystem.BLL.ViewModels.Invitation;
using EventManagementSystem.BLL.ViewModels.Notification;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EventManagementSystem.BLL.ViewModels.Person
{
    public record PersonDetailsVM
    {
        public string AppUserId { get; set; } = string.Empty;
        public string FullName { get; set; } = string.Empty; // From AppUser.Fullname
        public string Email { get; set; } = string.Empty;     // From AppUser.Email
        public string UserName { get; set; } = string.Empty;  // From AppUser.UserName

        // Additional properties you might have on AppUser or extend on Person
        // public string? PhoneNumber { get; set; } // From AppUser.PhoneNumber
        // public DateTime? DateOfBirth { get; set; } // If Person entity had this
        // public string? Address { get; set; } // If Person entity had this

        // Collections for related entities (might be summary VMs if full details are too much)
        public ICollection<InvitationListVM> Invitations { get; set; } = new List<InvitationListVM>();
        public ICollection<NotificationListVM> Notifications { get; set; } = new List<NotificationListVM>();
        public ICollection<FeedbackListVM> Feedbacks { get; set; } = new List<FeedbackListVM>();
    }
}
