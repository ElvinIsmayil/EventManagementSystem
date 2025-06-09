using System.ComponentModel.DataAnnotations;

namespace EventManagementSystem.BLL.ViewModels.Account
{
    public class ForgotPasswordVM
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }
    }
}
