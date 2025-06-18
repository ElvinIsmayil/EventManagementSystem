using EventManagementSystem.BLL.ViewModels.Profile;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;

namespace EventManagementSystem.BLL.Services.Interfaces
{
    public interface IProfileService
    {
        Task<ProfileHeaderVM> GetUserProfileHeaderAsync(string userId);
        Task<ProfileUpdateVM> GetUserProfileForUpdateAsync(string userId);
        Task<bool> UpdateUserProfileAsync(string userId, ProfileUpdateVM model);
        Task<(string? imageUrl, List<string> validationErrors)> UploadProfilePictureAsync(string userId, IFormFile imageFile);
        Task<bool> DeleteProfilePictureAsync(string userId);
        Task<ProfileDetailsVM> GetProfileDetailsAsync(string userId);
        Task<IdentityResult> ChangePasswordAsync(ChangePasswordVM model);

    }
}
