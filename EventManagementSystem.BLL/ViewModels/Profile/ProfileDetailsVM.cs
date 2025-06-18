using System.ComponentModel.DataAnnotations;

namespace EventManagementSystem.BLL.ViewModels.Profile
{
    public record ProfileDetailsVM
    {
        public string Id { get; set; }

        [Display(Name = "Full Name")]
        public string Fullname { get; set; } = string.Empty;

        [Display(Name = "Profile Picture")]
        public string ImageUrl { get; set; } = "/images/default-profile.jpg";

        [Display(Name = "Email")]
        public string Email { get; set; } = string.Empty;

        [Display(Name = "Email Confirmed")]
        public bool EmailConfirmed { get; set; }

        [Display(Name = "Primary Role")]
        public string Role { get; set; } = "User";

        [Display(Name = "Phone Number")]
        public string? PhoneNumber { get; set; }

        [Display(Name = "Date of Birth")]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime DateOfBirth { get; set; }

        [Display(Name = "Account Status")]
        public string AccountStatus => IsActive ? "Active" : "Inactive";
        public bool IsActive { get; set; }

        [Display(Name = "Member Since")]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd HH:mm}", ApplyFormatInEditMode = true)]
        public DateTime CreatedAt { get; set; }

        [Display(Name = "Last Login")]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd HH:mm}", ApplyFormatInEditMode = true)]
        public DateTime? LastLoginDate { get; set; }

        // You might add properties for Person or Organizer details if they are directly related and relevant for display
        // Example:
        // [Display(Name = "Person Details")]
        // public PersonDetailsVM? PersonDetails { get; set; }
    }
}
