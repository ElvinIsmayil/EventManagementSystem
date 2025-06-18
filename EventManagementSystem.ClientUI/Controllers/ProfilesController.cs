using EventManagementSystem.BLL.Services.Interfaces;
using EventManagementSystem.BLL.ViewModels.Profile;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace EventManagementSystem.ClientUI.Controllers
{
    public class ProfilesController : Controller
    {
        private readonly IProfileService _profileService;

        public ProfilesController(IProfileService profileService)
        {
            _profileService = profileService;
        }

        private string GetUserId()
        {
            return User.FindFirstValue(ClaimTypes.NameIdentifier);
        }

        [HttpGet]
        public async Task<IActionResult> Details()
        {
            var userId = GetUserId();
            // Fetch all necessary data for the comprehensive profile page
            var profileDetails = await _profileService.GetProfileDetailsAsync(userId);
            var profileUpdate = await _profileService.GetUserProfileForUpdateAsync(userId);

            // Create the composite ViewModel
            var model = new ProfilePageVM
            {
                ProfileDetails = profileDetails,
                ProfileUpdate = profileUpdate,
                ChangePassword = new ChangePasswordVM { UserId = userId }, // Initialize for the form
                ProfilePictureUpload = new ProfilePictureUploadVM { CurrentImageUrl = profileUpdate?.CurrentImageUrl }
            };

            if (profileDetails == null || profileUpdate == null)
            {
                // Handle cases where profile data cannot be retrieved (e.g., user not found)
                TempData["ErrorMessage"] = "Could not load your profile. Please try again.";
                return RedirectToHomeOrErrorPage(); // Or return NotFound() if appropriate
            }

            return View(model);
        }

        // Keep existing POST actions, they will just redirect back to Details
        // after processing.

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Update(ProfileUpdateVM profileUpdateVM)
        {
            // The Update action is called from a form on the Details page
            // We need to ensure we have the correct user ID here.
            var userId = GetUserId();

            if (!ModelState.IsValid)
            {
                // If validation fails, we need to return to the Details view
                // but re-populate the *entire* ProfilePageVM to display correctly.
                // This means refetching other parts of the profile.
                var profileDetails = await _profileService.GetProfileDetailsAsync(userId);
                var changePassword = new ChangePasswordVM { UserId = userId };
                var profilePictureUpload = new ProfilePictureUploadVM { CurrentImageUrl = (await _profileService.GetUserProfileForUpdateAsync(userId))?.CurrentImageUrl };

                var model = new ProfilePageVM
                {
                    ProfileDetails = profileDetails,
                    ProfileUpdate = profileUpdateVM, // Use the submitted VM with errors
                    ChangePassword = changePassword,
                    ProfilePictureUpload = profilePictureUpload
                };
                return View(nameof(Details), model);
            }

            var result = await _profileService.UpdateUserProfileAsync(userId, profileUpdateVM);
            if (!result)
            {
                ModelState.AddModelError("", "Failed to update profile. Please try again.");
                // Same re-population logic as above for displaying errors
                var profileDetails = await _profileService.GetProfileDetailsAsync(userId);
                var changePassword = new ChangePasswordVM { UserId = userId };
                var profilePictureUpload = new ProfilePictureUploadVM { CurrentImageUrl = (await _profileService.GetUserProfileForUpdateAsync(userId))?.CurrentImageUrl };
                var model = new ProfilePageVM
                {
                    ProfileDetails = profileDetails,
                    ProfileUpdate = profileUpdateVM,
                    ChangePassword = changePassword,
                    ProfilePictureUpload = profilePictureUpload
                };
                return View(nameof(Details), model);
            }

            TempData["Success"] = "Profile updated successfully.";
            return RedirectToAction(nameof(Details));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ChangePassword(ChangePasswordVM changePasswordVM)
        {
            var userId = GetUserId();
            changePasswordVM.UserId = userId; // Ensure userId is set

            if (!ModelState.IsValid)
            {
                // Re-populate other parts of the model for the Details view
                var profileDetails = await _profileService.GetProfileDetailsAsync(userId);
                var profileUpdate = await _profileService.GetUserProfileForUpdateAsync(userId);
                var profilePictureUpload = new ProfilePictureUploadVM { CurrentImageUrl = profileUpdate?.CurrentImageUrl };

                var model = new ProfilePageVM
                {
                    ProfileDetails = profileDetails,
                    ProfileUpdate = profileUpdate,
                    ChangePassword = changePasswordVM, // Use the submitted VM with errors
                    ProfilePictureUpload = profilePictureUpload
                };
                return View(nameof(Details), model);
            }

            try
            {
                var result = await _profileService.ChangePasswordAsync(changePasswordVM);

                if (!result.Succeeded)
                {
                    foreach (var error in result.Errors)
                    {
                        ModelState.AddModelError(string.Empty, error.Description);
                    }
                    // Re-populate and return to Details view with errors
                    var profileDetails = await _profileService.GetProfileDetailsAsync(userId);
                    var profileUpdate = await _profileService.GetUserProfileForUpdateAsync(userId);
                    var profilePictureUpload = new ProfilePictureUploadVM { CurrentImageUrl = profileUpdate?.CurrentImageUrl };

                    var model = new ProfilePageVM
                    {
                        ProfileDetails = profileDetails,
                        ProfileUpdate = profileUpdate,
                        ChangePassword = changePasswordVM,
                        ProfilePictureUpload = profilePictureUpload
                    };
                    return View(nameof(Details), model);
                }

                TempData["SuccessMessage"] = "Password changed successfully.";
                return RedirectToAction(nameof(Details));
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, "An unexpected error occurred while changing your password. Please try again.");
                // Re-populate and return to Details view with errors
                var profileDetails = await _profileService.GetProfileDetailsAsync(userId);
                var profileUpdate = await _profileService.GetUserProfileForUpdateAsync(userId);
                var profilePictureUpload = new ProfilePictureUploadVM { CurrentImageUrl = profileUpdate?.CurrentImageUrl };

                var model = new ProfilePageVM
                {
                    ProfileDetails = profileDetails,
                    ProfileUpdate = profileUpdate,
                    ChangePassword = changePasswordVM,
                    ProfilePictureUpload = profilePictureUpload
                };
                return View(nameof(Details), model);
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> UploadPicture(ProfilePictureUploadVM model)
        {
            string userId = GetUserId();

            if (model.NewImageFile == null || model.NewImageFile.Length == 0)
            {
                ModelState.AddModelError(string.Empty, "Please select an image file to upload.");
                // Re-populate other parts of the model for the Details view
                var profileDetails = await _profileService.GetProfileDetailsAsync(userId);
                var profileUpdate = await _profileService.GetUserProfileForUpdateAsync(userId);
                var changePassword = new ChangePasswordVM { UserId = userId };
                model.CurrentImageUrl = profileUpdate?.CurrentImageUrl; // Ensure current image URL is still displayed

                var pageModel = new ProfilePageVM
                {
                    ProfileDetails = profileDetails,
                    ProfileUpdate = profileUpdate,
                    ChangePassword = changePassword,
                    ProfilePictureUpload = model // Use the submitted model with errors
                };
                return View(nameof(Details), pageModel);
            }

            try
            {
                var (imageUrl, validationErrors) = await _profileService.UploadProfilePictureAsync(userId, model.NewImageFile);

                if (validationErrors.Any())
                {
                    foreach (var error in validationErrors)
                    {
                        ModelState.AddModelError(string.Empty, error);
                    }
                    // Re-populate and return to Details view with errors
                    var profileDetails = await _profileService.GetProfileDetailsAsync(userId);
                    var profileUpdate = await _profileService.GetUserProfileForUpdateAsync(userId);
                    var changePassword = new ChangePasswordVM { UserId = userId };
                    model.CurrentImageUrl = profileUpdate?.CurrentImageUrl;

                    var pageModel = new ProfilePageVM
                    {
                        ProfileDetails = profileDetails,
                        ProfileUpdate = profileUpdate,
                        ChangePassword = changePassword,
                        ProfilePictureUpload = model
                    };
                    return View(nameof(Details), pageModel);
                }

                TempData["SuccessMessage"] = "Profile picture uploaded successfully!";
                return RedirectToAction(nameof(Details));
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, "An unexpected error occurred during profile picture upload. Please try again.");
                // Re-populate and return to Details view with errors
                var profileDetails = await _profileService.GetProfileDetailsAsync(userId);
                var profileUpdate = await _profileService.GetUserProfileForUpdateAsync(userId);
                var changePassword = new ChangePasswordVM { UserId = userId };
                model.CurrentImageUrl = profileUpdate?.CurrentImageUrl;

                var pageModel = new ProfilePageVM
                {
                    ProfileDetails = profileDetails,
                    ProfileUpdate = profileUpdate,
                    ChangePassword = changePassword,
                    ProfilePictureUpload = model
                };
                return View(nameof(Details), pageModel);
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeletePicture()
        {
            try
            {
                var userId = GetUserId();
                var success = await _profileService.DeleteProfilePictureAsync(userId);

                if (!success)
                {
                    TempData["ErrorMessage"] = "Failed to delete profile picture. Please try again.";
                }
                else
                {
                    TempData["SuccessMessage"] = "Profile picture deleted successfully.";
                }

                return RedirectToAction(nameof(Details));
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = "An unexpected error occurred while deleting your profile picture. Please try again.";
                return RedirectToAction(nameof(Details));
            }
        }

        // Helper method for redirection in case of initial data retrieval failure
        private IActionResult RedirectToHomeOrErrorPage()
        {
            // You can customize this based on your application's error handling
            return RedirectToAction("Index", "Home"); // Example: redirect to home page
        }
    }
}