using EventManagementSystem.BLL.Infrastructure.Interfaces;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;

namespace EventManagementSystem.BLL.Infrastructure.Implementations
{
    public class ImageService : IImageService
    {
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly string[] _allowedImageExtensions = { ".jpg", ".jpeg", ".png" };
        private const long MaxFileSize = 5 * 1024 * 1024;

        public ImageService(IWebHostEnvironment webHostEnvironment)
        {
            _webHostEnvironment = webHostEnvironment;
        }

        public async Task<(string? imageUrl, List<string> validationErrors)> SaveImageAsync(IFormFile? imageFile, string folderName, string? oldImageUrl = null)
        {
            var validationErrors = ValidateFileType(imageFile);
            if (validationErrors.Any())
            {
                return (null, validationErrors);
            }


            if (!string.IsNullOrEmpty(oldImageUrl))
            {
                DeleteImage(oldImageUrl);
            }

            string wwwRootPath = _webHostEnvironment.WebRootPath;
            string uploadPath = Path.Combine(wwwRootPath, "uploads", folderName);

            try
            {
                if (!Directory.Exists(uploadPath))
                {
                    Directory.CreateDirectory(uploadPath);
                }
            }
            catch (Exception)
            {
                validationErrors.Add("Server error: Could not create upload directory.");
                return (null, validationErrors);
            }

            string fileName = Guid.NewGuid().ToString() + Path.GetExtension(imageFile?.FileName);
            string filePath = Path.Combine(uploadPath, fileName);

            try
            {

                using (var fileStream = new FileStream(filePath, FileMode.Create))
                {
                    await imageFile!.CopyToAsync(fileStream);
                }
            }
            catch (Exception)
            {
                validationErrors.Add("Server error: Could not save image file.");
                return (null, validationErrors);
            }

            string newImageUrl = $"/uploads/{folderName}/{fileName}";
            return (newImageUrl, new List<string>());
        }

        public void DeleteImage(string? imageUrl)
        {
            if (string.IsNullOrEmpty(imageUrl)) return;

            string wwwRootPath = _webHostEnvironment.WebRootPath;
            string imagePath = Path.Combine(wwwRootPath, imageUrl.TrimStart('/'));

            if (File.Exists(imagePath))
            {
                try
                {
                    File.Delete(imagePath);
                }
                catch (IOException ex)
                {
                    Console.WriteLine($"Error deleting image: {ex.Message}");
                }
            }
        }

        public List<string> ValidateFileType(IFormFile? imageFile)
        {
            var errors = new List<string>();

            if (imageFile == null)
            {
                errors.Add("Image file is required.");
                return errors;
            }

            if (imageFile.Length == 0)
            {
                errors.Add("The uploaded file is empty.");
                return errors;
            }

            if (imageFile.Length > MaxFileSize)
            {
                errors.Add($"File size exceeds the limit of {MaxFileSize / (1024 * 1024)} MB.");
            }

            var fileExtension = Path.GetExtension(imageFile.FileName)?.ToLowerInvariant();
            if (string.IsNullOrEmpty(fileExtension) || !_allowedImageExtensions.Contains(fileExtension))
            {
                errors.Add($"Invalid file type. Only {string.Join(", ", _allowedImageExtensions)} are allowed.");
            }

            return errors;
        }

        public async Task<(List<string> imageUrls, List<string> validationErrors)> SaveMultipleImagesAsync(List<IFormFile> imageFiles, string folderName)
        {
            var savedImageUrls = new List<string>();
            var allValidationErrors = new List<string>();

            if (imageFiles == null || !imageFiles.Any())
            {
                allValidationErrors.Add("No image files provided.");
                return (savedImageUrls, allValidationErrors);
            }

            // Check for general folder creation issues once
            string wwwRootPath = _webHostEnvironment.WebRootPath;
            string uploadPath = Path.Combine(wwwRootPath, "uploads", folderName);
            try
            {
                if (!Directory.Exists(uploadPath))
                {
                    Directory.CreateDirectory(uploadPath);
                }
            }
            catch (Exception)
            {
                allValidationErrors.Add("Server error: Could not create upload directory for multiple images.");
                return (savedImageUrls, allValidationErrors);
            }

            foreach (var imageFile in imageFiles)
            {
                var fileErrors = ValidateFileType(imageFile); // Re-use existing validation
                if (fileErrors.Any())
                {
                    allValidationErrors.AddRange(fileErrors);
                    continue; // Skip this file but continue processing others
                }

                string fileName = Guid.NewGuid().ToString() + Path.GetExtension(imageFile.FileName);
                string filePath = Path.Combine(uploadPath, fileName);

                try
                {
                    using (var fileStream = new FileStream(filePath, FileMode.Create))
                    {
                        await imageFile.CopyToAsync(fileStream);
                    }
                    savedImageUrls.Add($"/uploads/{folderName}/{fileName}");
                }
                catch (Exception ex)
                {
                    allValidationErrors.Add($"Server error: Could not save image file '{imageFile.FileName}'. Error: {ex.Message}");
                }
            }

            return (savedImageUrls, allValidationErrors);
        }
    }
}
