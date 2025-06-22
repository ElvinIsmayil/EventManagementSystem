using EventManagementSystem.BLL.Services.Interfaces;
using EventManagementSystem.BLL.ViewModels.Profile;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace EventManagementSystem.ClientUI.ViewComponents
{
    public class UserProfileViewComponent : ViewComponent
    {

        private readonly IProfileService _profileService;

        public UserProfileViewComponent(IProfileService profileService)
        {
            _profileService = profileService;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var defaultModel = new ProfileHeaderVM
            {
                Fullname = "Guest",
                ImageUrl = "/images/default-profile.jpg"
            };

            var claimsPrincipal = (ClaimsPrincipal)User;

            if (claimsPrincipal.Identity.IsAuthenticated)
            {
                try
                {
                    var userId = claimsPrincipal.FindFirstValue(ClaimTypes.NameIdentifier);

                    if (string.IsNullOrEmpty(userId))
                    {
                        return View(defaultModel);
                    }

                    var profileHeader = await _profileService.GetUserProfileHeaderAsync(userId);

                    if (string.IsNullOrEmpty(profileHeader.ImageUrl))
                    {
                        profileHeader.ImageUrl = "/images/default-profile.jpg";
                    }

                    return View(profileHeader);
                }
                catch (Exception)
                {
                    return View(defaultModel);
                }
            }

            return View(defaultModel);
        }
    }
}