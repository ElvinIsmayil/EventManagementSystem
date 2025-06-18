using System.ComponentModel.DataAnnotations;

namespace EventManagementSystem.BLL.ViewModels.Auth
{
    public record ForgotPasswordVM
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }
    }
}
