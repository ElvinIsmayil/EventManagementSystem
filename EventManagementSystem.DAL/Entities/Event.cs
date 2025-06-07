using EventManagementSystem.DAL.Entities.Common;

namespace EventManagementSystem.DAL.Entities
{
    public class Event : BaseEntity
    {
        public string Title { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }

        public ICollection<EventPhoto> EventPhotos { get; set; } = new List<EventPhoto>();

        public int EventTypeId { get; set; }
        public int LocationId { get; set; }
        public int OrganizerId { get; set; }

        public Organizer Organizer { get; set; } = null!;
        public EventType EventType { get; set; } = null!;
        public Location Location { get; set; } = null!;

        public List<Invitation> Invitations { get; set; } = new List<Invitation>();
        public List<Participation> Participations { get; set; } = new List<Participation>();
        public ICollection<Feedback> Feedbacks { get; set; } = new List<Feedback>();

    }
}
