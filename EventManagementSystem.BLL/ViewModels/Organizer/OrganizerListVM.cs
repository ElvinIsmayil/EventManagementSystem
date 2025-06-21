using System.ComponentModel.DataAnnotations;

namespace EventManagementSystem.BLL.ViewModels.Organizer
{
    public record OrganizerListVM
    {
        public int Id { get; set; }

        [Display(Name = "Organizer Name")]
        // This will typically come from AppUser.UserName or a combined Name property
        public string Name { get; set; } = string.Empty;

        [Display(Name = "Public Email")]
        public string PublicEmail { get; set; } = string.Empty;

        [Display(Name = "Public Phone")]
        public string PublicPhoneNumber { get; set; } = string.Empty;

        [Display(Name = "Average Rating")]
        public int AverageRating { get; set; } = 0;

        [Display(Name = "Approved")]
        public bool IsApproved { get; set; } = false;

        [Display(Name = "Events Organized")]
        public int OrganizedEventsCount { get; set; }
        public string UserId { get; set; } = string.Empty;
    }
}
