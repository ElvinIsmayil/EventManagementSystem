using System.ComponentModel.DataAnnotations;

namespace EventManagementSystem.BLL.ViewModels.Invitation
{
    public record InvitationCreateVM
    {
        [Required]
        public int EventId { get; set; }

        [Required]
        public int PersonId { get; set; }
    }
}
