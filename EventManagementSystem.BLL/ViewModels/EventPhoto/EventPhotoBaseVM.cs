using System.ComponentModel.DataAnnotations;

namespace EventManagementSystem.BLL.ViewModels.EventPhoto
{
    public record EventPhotoBaseVM
    {
        [Required(ErrorMessage = "Photo URL is required.")]
        [Url(ErrorMessage = "Please enter a valid URL.")]
        [StringLength(500, ErrorMessage = "URL cannot exceed 500 characters.")]
        public string Url { get; set; } = string.Empty;

        [StringLength(255, ErrorMessage = "File Name cannot exceed 255 characters.")]
        public string? FileName { get; set; }

        [StringLength(500, ErrorMessage = "Description cannot exceed 500 characters.")]
        public string? Description { get; set; }

        [Range(0, int.MaxValue, ErrorMessage = "Order must be a non-negative number.")]
        public int Order { get; set; } = 0;
    }
}
