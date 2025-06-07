using EventManagementSystem.DAL.Entities.Common;

namespace EventManagementSystem.DAL.Entities
{
    public class EventPhoto : BaseEntity
    {
        public string Url { get; set; } = string.Empty;
        public string? FileName { get; set; }
        public string? Description { get; set; }
        public int Order { get; set; } = 0;

        public int EventId { get; set; }
        public Event Event { get; set; } = null!;
    }
}
