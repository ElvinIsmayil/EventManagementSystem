using System.ComponentModel.DataAnnotations;

namespace EventManagementSystem.BLL.ViewModels.Organizer
{
    public record OrganizerCreateVM : OrganizerBaseVM
    {
        [Display(Name = "Associated User ID")]
        [Required(ErrorMessage = "Associated User ID is required.")]
        [StringLength(450, ErrorMessage = "User ID cannot exceed 450 characters.")] // Assuming string GUID or similar
        public string UserId { get; set; } = string.Empty;
    }
}
