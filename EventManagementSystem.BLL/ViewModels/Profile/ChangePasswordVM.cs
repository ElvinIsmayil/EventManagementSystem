using System.ComponentModel.DataAnnotations;

namespace EventManagementSystem.BLL.ViewModels.Profile
{
    public record ChangePasswordVM
    {
        public string UserId { get; set; } = null!;
        [Required(ErrorMessage = "Current password is required.")]
        [DataType(DataType.Password)]
        [Display(Name = "Current Password")]
        public string CurrentPassword { get; set; } = null!;
        [Required(ErrorMessage = "New password is required.")]
        [DataType(DataType.Password)]
        [Display(Name = "New Password")]
        public string NewPassword { get; set; } = null!;

    }
}
