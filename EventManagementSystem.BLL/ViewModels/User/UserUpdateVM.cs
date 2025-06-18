using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;

namespace EventManagementSystem.BLL.ViewModels.User
{
    public record UserUpdateVM
    {
        [Required]
        public string Id { get; set; } = null!;

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


    }
}
