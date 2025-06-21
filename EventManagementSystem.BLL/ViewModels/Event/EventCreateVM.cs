using EventManagementSystem.BLL.ViewModels.EventPhoto;
using System.ComponentModel.DataAnnotations;

namespace EventManagementSystem.BLL.ViewModels.Event
{
    public record EventCreateVM
    {
        [Required(ErrorMessage = "Title is required.")]
        [StringLength(200, ErrorMessage = "Title cannot exceed 200 characters.")]
        public string Title { get; set; } = string.Empty;

        [StringLength(1000, ErrorMessage = "Description cannot exceed 1000 characters.")]
        public string Description { get; set; } = string.Empty;

        [Required(ErrorMessage = "Start Date is required.")]
        [DataType(DataType.DateTime)]
        public DateTime StartDate { get; set; }

        [Required(ErrorMessage = "End Date is required.")]
        [DataType(DataType.DateTime)]
        public DateTime EndDate { get; set; }

        [Required(ErrorMessage = "Event Type is required.")]
        public int EventTypeId { get; set; }

        [Required(ErrorMessage = "Location is required.")]
        public int LocationId { get; set; }

        [Required(ErrorMessage = "Organizer is required.")]
        public int OrganizerId { get; set; }

        public List<EventPhotoCreateVM> EventPhotos { get; set; } = new List<EventPhotoCreateVM>();

    }
}
