using EventManagementSystem.BLL.ViewModels.EventPhoto;
using System.ComponentModel.DataAnnotations;

namespace EventManagementSystem.BLL.ViewModels.Event
{
    public record EventUpdateVM : EventCreateVM
    {
        [Required(ErrorMessage = "Id is required for updating an event.")]
        public int Id { get; set; }

        public new List<EventPhotoUpdateVM> EventPhotos { get; set; } = new List<EventPhotoUpdateVM>();
    }
}
