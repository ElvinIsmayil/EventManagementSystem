using EventManagementSystem.BLL.Services.Interfaces;
using EventManagementSystem.BLL.ViewModels.User;
using Microsoft.AspNetCore.Mvc;

namespace EventManagementSystem.ServerUI.ServerUI.Controllers
{
    public class UsersController : Controller
    {
        private readonly IUserService _userService;

        public UsersController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var users = await _userService.GetAllAsync();
            return View(users);
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(UserCreateVM userCreateVM)
        {
            if (!ModelState.IsValid)
            {
                return View(userCreateVM);
            }
            var user = await _userService.AddAsync(userCreateVM);
            if (user == null)
            {
                ModelState.AddModelError("", "User creation failed.");
                return View(userCreateVM);
            }
            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public async Task<IActionResult> Details(string id)
        {
            if (string.IsNullOrWhiteSpace(id))
                return BadRequest();

            var userDetailVM = await _userService.GetByIdAsync(id);
            if (userDetailVM == null)
            {
                return NotFound();
            }

            return View(userDetailVM);
        }

        [HttpGet]
        public async Task<IActionResult> Update(string id)
        {
            if (string.IsNullOrWhiteSpace(id))
                return BadRequest();
            var userUpdateVM = await _userService.GetUpdateByIdAsync(id);
            if (userUpdateVM == null)
            {
                return NotFound();
            }
            return View(userUpdateVM);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Update(UserUpdateVM userUpdateVM)
        {
            if (!ModelState.IsValid)
            {
                return View(userUpdateVM);
            }
            var user = await _userService.UpdateAsync(userUpdateVM);
            if (user == null)
            {
                ModelState.AddModelError("", "User update failed.");
                return View(userUpdateVM);
            }
            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(string id)
        {
            try
            {
                var user = await _userService.GetByIdAsync(id);
                if (user == null)
                {
                    return Json(new { success = false, message = "Event Type not found or already deleted." });
                }

                var result = await _userService.DeleteAsync(id);

                if (!result)
                {
                    return Json(new { success = false, message = "Failed to delete event type. It might be in use or protected." });
                }

                return Json(new { success = true, message = "Event Type deleted successfully." });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = "An unexpected error occurred while deleting the event type. Please try again later." });
            }

        }
    }
}
