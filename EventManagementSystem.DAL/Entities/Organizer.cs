using EventManagementSystem.DAL.Entities.Common;

namespace EventManagementSystem.DAL.Entities
{
    public class Organizer : BaseEntity
    {
        public string Name { get; set; } = string.Empty;
        public string Surname { get; set; } = string.Empty;
        public string Fullname => $"{Name} {Surname}";
        public string Email { get; set; } = string.Empty;
        public string PhoneNumber { get; set; } = string.Empty;
        public bool IsApproved { get; set; } = false;

        public string AppUserId { get; set; } = string.Empty;
        public AppUser AppUser { get; set; } = null!;

        public List<Event> OrganizedEvents { get; set; } = new List<Event>();

    }
}
