using EventManagementSystem.BLL.ViewModels.Event;
using EventManagementSystem.BLL.ViewModels.LocationPhoto;

namespace EventManagementSystem.BLL.ViewModels.Location
{
    public record LocationDetailsVM
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string Address { get; set; } = string.Empty;
        public int Capacity { get; set; }

        public ICollection<LocationPhotoDetailsVM> LocationPhotos { get; set; } = new List<LocationPhotoDetailsVM>();
        public ICollection<EventSummaryVM> Events { get; set; } = new List<EventSummaryVM>();
    }
}
