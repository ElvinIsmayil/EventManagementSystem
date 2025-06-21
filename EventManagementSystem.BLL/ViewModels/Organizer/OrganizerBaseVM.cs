using System.ComponentModel.DataAnnotations;

namespace EventManagementSystem.BLL.ViewModels.Organizer
{
    public record OrganizerBaseVM
    {
        [Display(Name = "Public Email")]
        [Required(ErrorMessage = "Public Email is required.")]
        [EmailAddress(ErrorMessage = "Invalid Email Address.")]
        [StringLength(255, ErrorMessage = "Public Email cannot exceed 255 characters.")]
        public string PublicEmail { get; set; } = string.Empty;

        [Display(Name = "Public Phone Number")]
        [Phone(ErrorMessage = "Invalid Phone Number.")]
        [StringLength(50, ErrorMessage = "Public Phone Number cannot exceed 50 characters.")]
        public string PublicPhoneNumber { get; set; } = string.Empty;

        [Display(Name = "Public Website")]
        [Url(ErrorMessage = "Invalid Website URL.")]
        [StringLength(500, ErrorMessage = "Public Website URL cannot exceed 500 characters.")]
        public string PublicWebsite { get; set; } = string.Empty;

    }
}
