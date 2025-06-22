using EventManagementSystem.DAL.Enums;
using System.ComponentModel.DataAnnotations;

namespace EventManagementSystem.BLL.ViewModels.Invitation
{
    public record InvitationUpdateVM
    {
        public int Id { get; set; }

        [Required]
        public InvitationStatus Status { get; set; } = InvitationStatus.Pending;
    }
}
