
using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;

namespace EventManagementSystem.BLL.ViewModels.LocationPhoto
{
    public record LocationPhotoCreateVM
    {
        [Required(ErrorMessage = "Please upload an image.")]
        public IFormFile File { get; set; } = null!;

        [StringLength(500, ErrorMessage = "Description cannot exceed 500 characters.")]
        public string? Description { get; set; }

        [Range(0, int.MaxValue, ErrorMessage = "Order must be a non-negative number.")]
        public int Order { get; set; }
    }
}
