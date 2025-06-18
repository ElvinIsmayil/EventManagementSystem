using EventManagementSystem.BLL.Infrastructure.Interfaces;
using EventManagementSystem.BLL.Services.Interfaces;
using EventManagementSystem.BLL.ViewModels.Profile;
using EventManagementSystem.DAL.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;

namespace EventManagementSystem.BLL.Services.Implementations
{
    public class ProfileService : IProfileService
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly IImageService _imageService;
        private const string FolderName = "profile_pictures";

        public ProfileService(IImageService imageService, UserManager<AppUser> userManager)
        {
            _imageService = imageService;
            _userManager = userManager;
        }

        public async Task<bool> DeleteProfilePictureAsync(string userId)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
            {
                throw new Exception("User not found.");
            }

            if (string.IsNullOrEmpty(user.ImageUrl))
            {
                return true;
            }

            try
            {
                _imageService.DeleteImage(user.ImageUrl); 
            }
            catch (Exception ex) 
            {
                Console.WriteLine($"Error deleting image file for user {userId}: {ex.Message}");
                return false;
            }

            user.ImageUrl = null;
            var result = await _userManager.UpdateAsync(user);

            if (!result.Succeeded)
            {
                throw new Exception($"Failed to update user profile after deleting image: {string.Join(", ", result.Errors.Select(e => e.Description))}");
            }

            return true; 
        }

        public async Task<ProfileUpdateVM> GetUserProfileForUpdateAsync(string userId)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
            {
                throw new Exception("User not found");
            }
            var profileUpdateVM = new ProfileUpdateVM
            {
                Name = user.Name,
                Surname = user.Surname,
                Email = user.Email,
                DateOfBirth = user.DateOfBirth,
                NewProfilePictureFile = null,
                PhoneNumber = user.PhoneNumber,
                CurrentImageUrl = user.ImageUrl
            };
            return profileUpdateVM;
        }

        public async Task<ProfileHeaderVM> GetUserProfileHeaderAsync(string userId)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
            {
                throw new Exception("User not found");
            }
            var profileHeaderVM = new ProfileHeaderVM
            {
                Id = user.Id,
                Fullname = user.Fullname,
                Email = user.Email,
                EmailConfirmed = user.EmailConfirmed,
                ImageUrl = user.ImageUrl
            };
            return profileHeaderVM;

        }

        public async Task<bool> UpdateUserProfileAsync(string userId, ProfileUpdateVM model)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
            {
                throw new Exception("User not found");
            }
            user.Name = model.Name;
            user.Surname = model.Surname;
            user.Email = model.Email;
            user.DateOfBirth = model.DateOfBirth;
            user.PhoneNumber = model.PhoneNumber;
            user.ImageUrl = model.CurrentImageUrl;
            user.UpdatedAt = DateTime.UtcNow;

            if (model.NewProfilePictureFile != null)
            {
                (var newImageUrl, List<string> validationErrors) = await _imageService.SaveImageAsync(model.NewProfilePictureFile, FolderName, userId);
                if (validationErrors.Any())
                {
                    throw new Exception($"Image upload failed: {string.Join(", ", validationErrors)}");
                }
                user.ImageUrl = newImageUrl;
            }
            var result = await _userManager.UpdateAsync(user);
            if (!result.Succeeded)
            {
                throw new Exception($"Failed to update user profile: {string.Join(", ", result.Errors.Select(e => e.Description))}");
            }

            return true;
        }

        public async Task<(string? imageUrl, List<string> validationErrors)> UploadProfilePictureAsync(string userId, IFormFile imageFile)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
            {
                return (null, new List<string> { "User not found." });
            }

            (var imageUrl, List<string> validationErrors) = await _imageService.SaveImageAsync(imageFile, FolderName, user.ImageUrl);

            if (validationErrors.Any())
            {
                return (null, validationErrors);
            }

            user.ImageUrl = imageUrl;
            var result = await _userManager.UpdateAsync(user);

            if (!result.Succeeded)
            {
                _imageService.DeleteImage(imageUrl);
                validationErrors.Add($"Failed to update user profile with new image: {string.Join(", ", result.Errors.Select(e => e.Description))}");
                return (null, validationErrors);
            }

            return (imageUrl, new List<string>());
        }

        public async Task<ProfileDetailsVM> GetProfileDetailsAsync(string userId)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
            {
                throw new Exception("User not found");
            }
            var profileDetailsVM = new ProfileDetailsVM
            {
                Id = user.Id,
                Fullname = user.Fullname,
                Email = user.Email,
                DateOfBirth = user.DateOfBirth,
                PhoneNumber = user.PhoneNumber,
                ImageUrl = user.ImageUrl,
                CreatedAt = user.CreatedAt
            };
            return profileDetailsVM;
        }

        public async Task<IdentityResult> ChangePasswordAsync(ChangePasswordVM model)
        {
            var user = await _userManager.FindByIdAsync(model.UserId);
            if (user == null)
            {
                throw new Exception("User not found");
            }
            var result = await _userManager.ChangePasswordAsync(user, model.CurrentPassword, model.NewPassword);
            if (!result.Succeeded)
            {
                throw new Exception($"Failed to change password: {string.Join(", ", result.Errors.Select(e => e.Description))}");
            }
            return result;
        }
    }
}
