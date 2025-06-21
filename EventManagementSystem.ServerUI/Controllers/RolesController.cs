using AutoMapper;
using EventManagementSystem.BLL.Infrastructure;
using EventManagementSystem.BLL.ViewModels.Role;
using EventManagementSystem.DAL.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Data;

namespace EventManagementSystem.ServerUI.ServerUI.Controllers
{
    public class RolesController : Controller
    {
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly UserManager<AppUser> _userManager;
        private readonly IMapper _mapper;

        public RolesController(RoleManager<IdentityRole> roleManager, UserManager<AppUser> userManager, IMapper mapper)
        {
            _roleManager = roleManager;
            _userManager = userManager;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var roles = await _roleManager.Roles?.ToListAsync();
            var roleVMs = roles.Select(role => new RoleListVM
            {
                Id = role.Id,
                Name = role.Name
            }).ToList();
            return View(roleVMs);
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> Details(string id)
        {
            var role = await _roleManager.Roles.FirstOrDefaultAsync(x => x.Id == id);
            if (role == null)
            {
                return NotFound();
            }

            var usersInRole = await _userManager.GetUsersInRoleAsync(role.Name);
            var usersInRoleVM = usersInRole.Select(user => new UserInRoleVM
            {
                Id = user.Id,
                Email = user.Email,
                FullName = user.Name + " " + user.Surname,
                JoinedDate = user.CreatedAt,
                ProfileImageUrl = user.ImageUrl
            }).OrderBy(u => u.FullName).ToList();

            var allUsers = await _userManager.Users.ToListAsync();
            var availableUsers = allUsers
                .Where(u => !usersInRole.Any(ur => ur.Id == u.Id))
                .Select(user => new UserInRoleVM
                {
                    Id = user.Id,
                    Email = user.Email,
                    FullName = user.Name + " " + user.Surname,
                    JoinedDate = user.CreatedAt,
                    ProfileImageUrl = user.ImageUrl
                })
                .OrderBy(u => u.FullName)
                .ToList();

            var model = _mapper.Map<RoleDetailsVM>(role);

            return View(model);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AssignRole(AssignRoleVM model)
        {
            if (!ModelState.IsValid)
            {
                var errors = string.Join(" ", ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage));
                TempData[AlertHelper.Error] = $"Invalid data provided: {errors}";
                return RedirectToAction("Details", new { id = model.RoleId });
            }

            if (string.IsNullOrEmpty(model.UserId))
            {
                TempData[AlertHelper.Error] = "Please select a user to assign.";
                return RedirectToAction(nameof(Details));
            }

            var user = await _userManager.FindByIdAsync(model.UserId);
            if (user == null)
            {
                TempData[AlertHelper.Error] = "User not found.";
                return RedirectToAction("Details", new { id = model.RoleId });
            }

            var role = await _roleManager.FindByNameAsync(model.RoleName);
            if (role == null)
            {
                TempData[AlertHelper.Error] = "Role not found.";
                return RedirectToAction("Details", new { id = model.RoleId });
            }

            if (await _userManager.IsInRoleAsync(user, role.Name))
            {
                TempData[AlertHelper.Error] = $"User '{user.Name} {user.Surname}' is already in the '{role.Name}' role.";
                return RedirectToAction("Details", new { id = model.RoleId });
            }

            var result = await _userManager.AddToRoleAsync(user, role.Name);
            if (result.Succeeded)
            {
                TempData[AlertHelper.Success] = $"User '{user.Name}   {user.Surname}' successfully assigned to '{role.Name}' role.";
                return RedirectToAction("Details", new { id = model.RoleId });
            }
            else
            {
                var errors = string.Join(" ", result.Errors.Select(e => e.Description));
                TempData[AlertHelper.Error] = $"Failed to assign user to role: {errors}";
                return RedirectToAction("Details", new { id = model.RoleId });
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> RemoveUserFromRole(string UserId, string RoleName)
        {
            // You don't need RoleId here if you're not redirecting.
            // If you need it for logging or other internal purposes, you can keep it,
            // but it won't be used for the response in this AJAX context.

            var user = await _userManager.FindByIdAsync(UserId);
            if (user == null)
            {
                return Json(new { success = false, message = "User not found." });
            }

            var role = await _roleManager.FindByNameAsync(RoleName);
            if (role == null)
            {
                return Json(new { success = false, message = "Role not found." });
            }

            // Add a null check for user.Name and user.Surname if they can be null
            string fullName = $"{user.Name} {user.Surname}".Trim();
            if (string.IsNullOrWhiteSpace(fullName))
            {
                fullName = user.UserName; // Fallback to username if name/surname are empty
            }

            if (!await _userManager.IsInRoleAsync(user, RoleName))
            {
                return Json(new { success = false, message = $"User '{fullName}' is not in the '{RoleName}' role." });
            }

            var result = await _userManager.RemoveFromRoleAsync(user, RoleName);
            if (result.Succeeded)
            {
                return Json(new { success = true, message = $"User '{fullName}' successfully removed from '{RoleName}' role." });
            }
            else
            {
                var errors = string.Join(" ", result.Errors.Select(e => e.Description));
                return Json(new { success = false, message = $"Failed to remove user from role: {errors}" });
            }
        }
    }
}
