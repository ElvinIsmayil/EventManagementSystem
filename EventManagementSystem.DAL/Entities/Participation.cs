using EventManagementSystem.DAL.Entities.Common;

namespace EventManagementSystem.DAL.Entities
{
    public class Participation : BaseEntity
    {
        public DateTime ParticipationDate { get; set; } = DateTime.Now;
        public DateTime CheckInDate { get; set; }
        public int SeatNumber { get; set; }
        public string ConfirmationCode { get; set; } = Guid.NewGuid().ToString();
        public bool IsConfirmed { get; set; } = false;

        public int InvitationId { get; set; }
        public Invitation Invitation { get; set; } = null!;
    }
}
