using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;

namespace EventManagementSystem.BLL.ViewModels.User
{
    public record UserCreateVM
    {
        [Required(ErrorMessage = "First Name is required.")]
        [Display(Name = "First Name")]
        [StringLength(50)]
        public string Name { get; set; } = null!;

        [Required(ErrorMessage = "Last Name is required.")]
        [Display(Name = "Last Name")]
        [StringLength(50)]
        public string Surname { get; set; } = null!;
        public DateTime? DateOfBirth { get; set; } = DateTime.UtcNow;

        [Required(ErrorMessage = "Email is required.")]
        [EmailAddress]
        public string Email { get; set; } = null!;
        public string? PhoneNumber { get; set; }

        [Display(Name = "Profile Image")]
        public string? ImageUrl { get; set; }
        public IFormFile? ImageFile { get; set; }
        public bool IsActive { get; set; } = true;
        public List<string?> Role { get; set; } = null!;

        [Required(ErrorMessage = "Password is required.")]
        [DataType(DataType.Password)]
        [StringLength(100, MinimumLength = 6, ErrorMessage = "Password must be at least 6 characters.")]
        public string Password { get; set; } = null!;

        [Compare("Password", ErrorMessage = "Passwords do not match.")]
        [DataType(DataType.Password)]
        [Display(Name = "Confirm Password")]
        public string ConfirmPassword { get; set; } = null!;
    }
}
