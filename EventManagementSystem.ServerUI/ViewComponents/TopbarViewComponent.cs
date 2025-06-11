using EventManagementSystem.BLL.ViewModels.User;
using EventManagementSystem.DAL.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace EventManagementSystem.ServerUI.ViewComponents;

public class TopbarViewComponent : ViewComponent
{
    private readonly UserManager<AppUser> _userManager;

    public TopbarViewComponent(UserManager<AppUser> userManager)
    {
        _userManager = userManager;
    }

    public async Task<IViewComponentResult> InvokeAsync()
    {
        var user = await _userManager.GetUserAsync(HttpContext.User);

        if (user == null)
        {
            var errorModel = new UserHeaderVM
            {
                Id = null,
                ImageUrl = "/media/avatars/blank.png",
                FullName = "Error User",
                Role = "N/A",
                Email = "user-error@example.com",
                EmailConfirmed = false
            };
            ViewBag.RoleBadgeColor = GetRoleBadgeColor(errorModel.Role);
            return View(errorModel);
        }

        var roles = await _userManager.GetRolesAsync(user);
        string displayRole = roles.FirstOrDefault() ?? "User";

        var userVM = new UserHeaderVM
        {
            Id = user.Id,
            FullName = user.Fullname,
            ImageUrl = user.ImageUrl ?? "/media/avatars/blank.png",
            Email = user.Email,
            EmailConfirmed = user.EmailConfirmed,
            Role = displayRole
        };

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
