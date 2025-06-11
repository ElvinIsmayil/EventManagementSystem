using EventManagementSystem.BLL.ViewModels.LocationPhoto;
using System.ComponentModel.DataAnnotations;

namespace EventManagementSystem.BLL.ViewModels.Location
{
    public record LocationCreateVM
    {
        [Required(ErrorMessage = "Location name is required.")]
        [StringLength(100, ErrorMessage = "Location name cannot exceed 100 characters.")]
        public string Name { get; set; } = string.Empty;

        [StringLength(500, ErrorMessage = "Description cannot exceed 500 characters.")]
        public string Description { get; set; } = string.Empty;

        [Required(ErrorMessage = "Address is required.")]
        [StringLength(200, ErrorMessage = "Address cannot exceed 200 characters.")]
        public string Address { get; set; } = string.Empty;

        [Range(1, 100000, ErrorMessage = "Capacity must be between 1 and 100,000.")]
        public int Capacity { get; set; }

        [Display(Name = "Location Photos")]
        public ICollection<LocationPhotoCreateVM>? Photos { get; set; } = new List<LocationPhotoCreateVM>();
    }
}
