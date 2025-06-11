using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;

namespace EventManagementSystem.BLL.ViewModels.LocationPhoto
{
    public record LocationPhotoUpdateVM
    {
        [Required]
        public int Id { get; set; }

        [StringLength(500, ErrorMessage = "Description cannot exceed 500 characters.")]
        public string? Description { get; set; }

        [Range(0, int.MaxValue, ErrorMessage = "Order must be a non-negative number.")]
        public int Order { get; set; } = 0;

        [Display(Name = "Replace Photo")]
        public IFormFile? NewFile { get; set; }
    }
}
