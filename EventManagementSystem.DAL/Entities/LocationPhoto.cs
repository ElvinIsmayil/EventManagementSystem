using EventManagementSystem.DAL.Entities.Common;

namespace EventManagementSystem.DAL.Entities
{
    public class LocationPhoto : BaseEntity
    {
        public string Url { get; set; } = string.Empty;
        public string? Description { get; set; }
        public int Order { get; set; } = 0;

        public int LocationId { get; set; }
        public Location Location { get; set; } = null!;
    }
}
