using System.ComponentModel.DataAnnotations;

namespace EventManagementSystem.BLL.ViewModels.Auth
{
    public record ConfirmEmailVM
    {
        [Required]
        public string UserId { get; set; }

        [Required]
        public string Token { get; set; }
    }
}
