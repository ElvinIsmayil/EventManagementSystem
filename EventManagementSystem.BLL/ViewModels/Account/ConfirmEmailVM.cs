using System.ComponentModel.DataAnnotations;

namespace EventManagementSystem.BLL.ViewModels.Account
{
    public class ConfirmEmailVM
    {
        [Required]
        public string UserId { get; set; }

        [Required]
        public string Token { get; set; }
    }
}
