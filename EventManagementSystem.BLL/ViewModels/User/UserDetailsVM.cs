using System.ComponentModel.DataAnnotations;

namespace EventManagementSystem.BLL.ViewModels.User
{
    public record UserDetailsVM
    {
        [Required]
        public string Id { get; set; } = null!;

        [Display(Name = "Name")]
        public string Name { get; set; } = null!;

        [Display(Name = "Surname")]
        public string Surname { get; set; } = null!;
        [Display(Name = "Full Name")]
        public string FullName => $"{Name} {Surname}";

        [EmailAddress]
        public string Email { get; set; } = null!;

        [Display(Name = "Email Confirmed")]
        public bool EmailConfirmed { get; set; }

        [Display(Name = "Profile Image")]
        public string? ImageUrl { get; set; } = "/media/avatars/blank.png";

        [Display(Name = "Joined Date")]
        [DataType(DataType.DateTime)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd HH:mm}")]
        public DateTime CreatedAt { get; set; }

        [Display(Name = "Last Login Date")]
        [DataType(DataType.DateTime)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd HH:mm}", NullDisplayText = "Never logged in")]
        public DateTime? LastLoginDate { get; set; }

        [Display(Name = "Role")]
        public List<string?> Role { get; set; }

        [Display(Name = "Phone Number")]
        public string? PhoneNumber { get; set; }

        [Display(Name = "Address")]
        public string? Address { get; set; }
    }
}

