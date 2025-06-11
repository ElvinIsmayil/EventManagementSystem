using EventManagementSystem.BLL.ViewModels.LocationPhoto;

namespace EventManagementSystem.BLL.ViewModels.Location
{
    public record LocationUpdateVM
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string Address { get; set; } = string.Empty;
        public int Capacity { get; set; }
        public ICollection<LocationPhotoUpdateVM> LocationPhotos { get; set; } = new List<LocationPhotoUpdateVM>();
        public ICollection<int> EventIds { get; set; } = new List<int>();

    }
}
