using EventManagementSystem.DAL.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks; // Required for async/await

namespace EventManagementSystem.ServerUI.Controllers
{
    public class DashboardController : Controller
    {
        private readonly UserManager<AppUser> _userManager;

        public DashboardController(UserManager<AppUser> userManager)
        {
            _userManager = userManager;
        }

        [HttpGet]
        public async Task<IActionResult> Index() // Made async
        {
            // Get the currently logged-in user
            var user = await _userManager.GetUserAsync(User);

            // Check if the user is logged in and the FullName property exists
            if (user != null && !string.IsNullOrEmpty(user.Fullname))
            {
                ViewData["FullName"] = user.Fullname;
            }
            else
            {
                // Optionally, provide a default or handle cases where FullName is not set
                ViewData["FullName"] = "Guest"; // Or an empty string
            }

            return View();
        }
    }
}