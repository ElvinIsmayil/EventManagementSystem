using System.ComponentModel.DataAnnotations;

namespace EventManagementSystem.BLL.ViewModels.Account
{
    public record ForgotPasswordVM
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }
    }
}
