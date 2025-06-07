using EventManagementSystem.DAL.Entities.Common;

namespace EventManagementSystem.DAL.Entities
{
    public class Location : BaseEntity
    {
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string Address { get; set; } = string.Empty;
        public int Capacity { get; set; }

        public ICollection<LocationPhoto> LocationPictures { get; set; } = new List<LocationPhoto>();

        public virtual ICollection<Event> Events { get; set; } = new List<Event>();
    }

}
