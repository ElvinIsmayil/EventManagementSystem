using EventManagementSystem.DAL.Entities.Common;
using EventManagementSystem.DAL.Enums;

namespace EventManagementSystem.DAL.Entities
{
    public class Person : BaseEntity
    {
        public string Name { get; set; } = string.Empty;
        public string Surname { get; set; } = string.Empty;
        public string Fullname => $"{Name} {Surname}";
        public string Email { get; set; } = string.Empty;
        public string PhoneNumber { get; set; } = string.Empty;
        public PersonType Role { get; set; }

        public string AppUserId { get; set; } = string.Empty;
        public AppUser AppUser { get; set; } = null!;

        public ICollection<Invitation> Invitations { get; set; } = new List<Invitation>();
        public ICollection<Notification> Notifications { get; set; } = new List<Notification>();
        public ICollection<Feedback> Feedbacks { get; set; } = new List<Feedback>();
    }
}
