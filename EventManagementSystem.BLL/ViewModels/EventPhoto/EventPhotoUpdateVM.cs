using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;

namespace EventManagementSystem.BLL.ViewModels.EventPhoto
{
    public record EventPhotoUpdateVM : EventPhotoBaseVM
    {
        [Required(ErrorMessage = "Id is required for identifying the photo to update.")]
        [Range(1, int.MaxValue, ErrorMessage = "Id must be a positive integer.")]
        public int Id { get; set; }
        [DataType(DataType.Upload)]
        public IFormFile? PhotoFile { get; set; }
    }
}
