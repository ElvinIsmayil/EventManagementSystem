using EventManagementSystem.BLL.ViewModels.Event;
using EventManagementSystem.BLL.ViewModels.User;
using System.ComponentModel.DataAnnotations;

namespace EventManagementSystem.BLL.ViewModels.Organizer
{
    public record OrganizerDetailsVM
    {
        public int Id { get; set; }

        [Display(Name = "Public Email")]
        public string PublicEmail { get; set; } = string.Empty;

        [Display(Name = "Public Phone Number")]
        public string PublicPhoneNumber { get; set; } = string.Empty;

        [Display(Name = "Public Website")]
        public string PublicWebsite { get; set; } = string.Empty;

        [Display(Name = "Average Rating")]
        public int AverageRating { get; set; } = 0;

        [Display(Name = "Is Approved")]
        public bool IsApproved { get; set; } = false;
        [Display(Name = "Associated User Account")]
        public UserDetailsVM User { get; set; } = null!;

        // Nested view model for AppUser details
        [Display(Name = "Number of Organized Events")]
        public int OrganizedEventsCount { get; set; }

        // Count of events organized by this organizer for summary
        public List<EventSummaryVM> OrganizedEvents { get; set; } = new List<EventSummaryVM>();

    }


}
