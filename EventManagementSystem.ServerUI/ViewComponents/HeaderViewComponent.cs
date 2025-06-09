using EventManagementSystem.BLL.ViewModels.User;
using EventManagementSystem.DAL.Entities; // Assuming AppUser is in DAL.Entities
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace EventManagementSystem.ServerUI.ViewComponents;

public class HeaderViewComponent : ViewComponent
{
    private readonly UserManager<AppUser> _userManager;

    public HeaderViewComponent(UserManager<AppUser> userManager)
    {
        _userManager = userManager;
    }

    public async Task<IViewComponentResult> InvokeAsync()
    {
        // Handle unauthenticated users
        if (!User.Identity.IsAuthenticated)
        {
            var guestModel = new UserHeaderVM
            {
                Id = null,
                ImageUrl = "/media/avatars/blank.png", // Always use default for guest
                FullName = "Guest User",
                Role = "Guest",
                Email = "guest@example.com",
                EmailConfirmed = false
            };
            ViewBag.RoleBadgeColor = GetRoleBadgeColor(guestModel.Role);
            return View(guestModel);
        }

        var user = await _userManager.GetUserAsync(HttpContext.User);

        // Handle authenticated user not found in UserManager
        if (user == null)
        {
            var notFoundModel = new UserHeaderVM
            {
                Id = null,
                ImageUrl = "/media/avatars/blank.png", // Always use default for unknown
                FullName = "Unknown User",
                Role = "N/A",
                Email = "error@example.com",
                EmailConfirmed = false
            };
            ViewBag.RoleBadgeColor = GetRoleBadgeColor(notFoundModel.Role);
            return View(notFoundModel);
        }

        // Get user roles
        var roles = await _userManager.GetRolesAsync(user);
        string displayRole = roles.FirstOrDefault() ?? "User"; // Default to "User" if no roles found

        var userVM = new UserHeaderVM
        {
            Id = user.Id,
            FullName = user.UserName, // Use UserName for FullName
            ImageUrl = "/media/avatars/blank.png", // Always use the default blank image
            Email = user.Email,
            EmailConfirmed = user.EmailConfirmed,
            Role = displayRole
        };

        // Pass the badge color to the view via ViewBag
        ViewBag.RoleBadgeColor = GetRoleBadgeColor(userVM.Role);

        return View(userVM);
    }

    // Helper function for badge color
    private string GetRoleBadgeColor(string role)
    {
        switch (role?.ToLower())
        {
            case "superadmin": return "danger";
            case "admin": return "primary";
            case "editor": return "info";
            case "user": return "success";
            case "guest": return "secondary"; // Added for guest users
            default: return "secondary";
        }
    }
}