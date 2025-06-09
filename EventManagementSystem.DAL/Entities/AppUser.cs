using Microsoft.AspNetCore.Identity;

namespace EventManagementSystem.DAL.Entities
{
    public class AppUser : IdentityUser
    {
        public string Name { get; set; } = string.Empty;
        public string Surname { get; set; } = string.Empty;
        public string Fullname => $"{Name} {Surname}";
        public string ImageUrl { get; set; } = string.Empty;
        public DateTime DateOfBirth { get; set; } = DateTime.UtcNow;
        public string? PhoneNumber { get; set; } = null;
        public bool IsActive { get; set; } = true;
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime? UpdatedAt { get; set; }
        public DateTime? LastLoginDate { get; set; } = null;

        public Person? Person { get; set; }
        public Organizer? Organizer { get; set; }
    }

}
