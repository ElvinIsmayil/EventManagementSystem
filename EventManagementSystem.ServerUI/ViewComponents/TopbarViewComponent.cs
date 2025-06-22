using EventManagementSystem.BLL.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims; // Add this using directive
using EventManagementSystem.BLL.ViewModels.Profile; // Make sure your UserProfileHeaderVM is imported

namespace EventManagementSystem.ServerUI.ViewComponents;

public class TopbarViewComponent : ViewComponent
{
    private readonly IProfileService _profileService;

    public TopbarViewComponent(IProfileService profileService)
    {
        _profileService = profileService;
    }

    public async Task<IViewComponentResult> InvokeAsync()
    {
        // Initialize userVM to null. This will be the default if not authenticated or profile not found.
        ProfileHeaderVM? userVM = null;

        // 1. Get the current user's ID only if authenticated
        // This is more robust than passing a null string to the service directly,
        // although the service should also handle null userId defensively.
        if (HttpContext.User.Identity.IsAuthenticated)
        {
            string? userId = HttpContext.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            // Only attempt to get the profile if a userId was found
            if (!string.IsNullOrEmpty(userId))
            {
                try
                {
                    userVM = await _profileService.GetUserProfileHeaderAsync(userId);
                }
                catch (Exception ex)
                {
                    // Log the exception here. This catch is for *unexpected* errors (e.g., DB connection issues),
                    // NOT for "user profile not found" which GetUserProfileHeaderAsync now handles by returning null.
                    // For example: _logger.LogError(ex, "Error fetching user profile for ID: {UserId}", userId);
                    // Ensure userVM remains null so the view handles the error gracefully.
                    userVM = null;
                }
            }
        }

        // --- Handle userVM being null ---
        // If userVM is null (either not authenticated or profile not found),
        // we can return a view with a null model, and the .cshtml will handle displaying "Sign In/Sign Up".
        // There's no need for the 'if (userVM.Id == null)' block here.
        // The check for userVM being null should happen directly in the Razor View.

        // Set RoleBadgeColor only if userVM is not null and has a role
        ViewBag.RoleBadgeColor = GetRoleBadgeColor(userVM?.Role); // Use null-conditional operator here

        return View(userVM);
    }

    private string GetRoleBadgeColor(string? role) // Make role parameter nullable
    {
        // Use null-conditional operator and null-coalescing operator for safety
        switch (role?.ToLower() ?? "default") // Use "default" for null or empty roles
        {
            case "superadmin": return "danger";
            case "admin": return "primary";
            case "organizer": return "info"; // Added for organizer role
            case "default": // This catches null, empty, or unhandled roles
            default: return "secondary";
        }
    }
}