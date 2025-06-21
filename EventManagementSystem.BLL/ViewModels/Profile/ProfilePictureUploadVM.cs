using Microsoft.AspNetCore.Http;

namespace EventManagementSystem.BLL.ViewModels.Profile
{
    public record ProfilePictureUploadVM
    {
        public IFormFile? NewImageFile { get; set; }
        public string? CurrentImageUrl { get; set; }
    }
}
