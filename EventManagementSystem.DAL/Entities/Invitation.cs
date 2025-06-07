using EventManagementSystem.DAL.Entities.Common;
using EventManagementSystem.DAL.Enums;

namespace EventManagementSystem.DAL.Entities
{
    public class Invitation : BaseEntity
    {
        public DateTime SentAt { get; set; } = DateTime.UtcNow;
        public string InvitationCode { get; set; } = Guid.NewGuid().ToString();
        public InvitationStatus Status { get; set; } = InvitationStatus.Pending;

        public int EventId { get; set; }
        public int PersonId { get; set; }

        public Event Event { get; set; } = null!;
        public Person Person { get; set; } = null!;

        public Participation Participation { get; set; } = null!;
    }
}
