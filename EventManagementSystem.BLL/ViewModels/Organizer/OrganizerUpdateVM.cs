using System.ComponentModel.DataAnnotations;

namespace EventManagementSystem.BLL.ViewModels.Organizer
{
    public record OrganizerUpdateVM : OrganizerBaseVM
    {
        [Required(ErrorMessage = "Id is required for updating an organizer.")]
        [Range(1, int.MaxValue, ErrorMessage = "Id must be a positive integer.")]
        public int Id { get; set; }

        [Display(Name = "Associated User ID")]
        [Required(ErrorMessage = "Associated User ID is required.")]
        [StringLength(450, ErrorMessage = "User ID cannot exceed 450 characters.")]
        public string UserId { get; set; } = string.Empty;

        [Display(Name = "Is Approved")]
        public bool IsApproved { get; set; } = false;
        // AverageRating is typically updated by the system, not directly by an update form.

    }
}
