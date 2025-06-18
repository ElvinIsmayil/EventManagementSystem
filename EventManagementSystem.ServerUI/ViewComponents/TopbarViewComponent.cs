using EventManagementSystem.BLL.Services.Interfaces;
using EventManagementSystem.BLL.ViewModels.Profile;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims; // Add this using directive

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
        string userId = null;
        if (HttpContext.User.Identity.IsAuthenticated)
        {
            userId = HttpContext.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        }

        var userVM = await _profileService.GetUserProfileHeaderAsync(userId);

        if (userVM.Id == null)
        {
        }

        ViewBag.RoleBadgeColor = GetRoleBadgeColor(userVM.Role);

        return View(userVM);
    }

    private string GetRoleBadgeColor(string role)
    {
        switch (role?.ToLower())
        {
            case "superadmin": return "danger";
            case "admin": return "primary";
            default: return "secondary";
        }
    }
}