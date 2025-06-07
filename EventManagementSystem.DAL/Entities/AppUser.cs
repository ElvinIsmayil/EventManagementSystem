using Microsoft.AspNetCore.Identity;

namespace EventManagementSystem.DAL.Entities
{
    public class AppUser : IdentityUser
    {
        public bool IsActive { get; set; } = true;
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime? UpdatedAt { get; set; }

        public Person? Person { get; set; }
        public Organizer? Organizer { get; set; }

    }
}
