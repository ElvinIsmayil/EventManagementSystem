using EventManagementSystem.DAL.Entities; 
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace EventManagementSystem.Areas.Admin.Controllers
{
    // [Authorize(Roles = "Admin")]
    public class DashboardController : Controller
    {
        private readonly UserManager<AppUser> _userManager;

        public DashboardController(UserManager<AppUser> userManager)
        {
            _userManager = userManager;
        }

        public async Task<IActionResult> Index()
        {
            var user = await _userManager.GetUserAsync(User);

            string displayName;
            if (user != null)
            {
                // Prioritize FullName, then UserName, then the general Identity Name
                if (!string.IsNullOrEmpty(user.Fullname)) // Assuming AppUser has a FullName property
                {
                    displayName = user.Fullname;
                }
                else
                {
                    displayName = user.UserName; // Fallback to username
                }
                displayName = $"Salam, {displayName}"; // Add the greeting
            }
            else
            {
                // If user object not found, fall back to the generic identity name (e.g., email or username)
                displayName = $"Salam, {User.Identity.Name}";
            }

            ViewData["FullName"] = displayName;

            return View();
        }
    }
}