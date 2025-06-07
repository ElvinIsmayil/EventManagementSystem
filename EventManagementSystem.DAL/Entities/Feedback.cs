using EventManagementSystem.DAL.Entities.Common;

namespace EventManagementSystem.DAL.Entities
{
    public class Feedback : BaseEntity
    {
        public int Rating { get; set; }
        public string? Comment { get; set; }
        public DateTime SubmittedAt { get; set; } = DateTime.UtcNow;

        public int PersonId { get; set; }
        public int EventId { get; set; }

        public Person Person { get; set; } = null!;
        public Event Event { get; set; } = null!;
    }
}
