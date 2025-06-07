using EventManagementSystem.DAL.Entities.Common;

namespace EventManagementSystem.DAL.Entities
{
    public class EventType : BaseEntity
    {
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;

        public ICollection<Event> Events { get; set; } = new List<Event>();
    }

}
