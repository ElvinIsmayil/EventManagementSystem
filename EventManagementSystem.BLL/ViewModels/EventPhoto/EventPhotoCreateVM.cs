using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;

namespace EventManagementSystem.BLL.ViewModels.EventPhoto
{
    public record EventPhotoCreateVM : EventPhotoBaseVM
    {
        public IFormFile? File { get; set; } = null!;
        [Required(ErrorMessage = "Please upload a photo file.")]
        [DataType(DataType.Upload)] // Hint for UI frameworks/documentation
        public IFormFile PhotoFile { get; set; } = null!; // Represents the uploaded file
    }
}
