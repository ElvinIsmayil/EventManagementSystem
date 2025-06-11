using Microsoft.AspNetCore.Http;

namespace EventManagementSystem.BLL.Infrastructure.Interfaces
{
    public interface IImageService
    {
        Task<(string? imageUrl, List<string> validationErrors)> SaveImageAsync(IFormFile? imageFile, string folderName, string? oldImageUrl = null);
        void DeleteImage(string imageUrl);
        List<string> ValidateFileType(IFormFile imageFile);

        Task<(List<string> imageUrls, List<string> validationErrors)> SaveMultipleImagesAsync(List<IFormFile> imageFiles, string folderName);

    }
}
