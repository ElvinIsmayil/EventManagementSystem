using EventManagementSystem.DAL.Entities.Common;

namespace EventManagementSystem.DAL.Entities
{
    public class Organizer : BaseEntity
    {
        public string PublicEmail { get; set; } = string.Empty;
        public string PublicPhoneNumber { get; set; } = string.Empty;
        public string PublicWebsite { get; set; } = string.Empty;
        public int AverageRating { get; set; } = 0;
        public bool IsApproved { get; set; } = false;
        public List<Event> OrganizedEvents { get; set; } = new List<Event>();

        public string AppUserId { get; set; } = string.Empty;
        public AppUser AppUser { get; set; } = null!;

    }
}
