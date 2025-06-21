using EventManagementSystem.BLL.Infrastructure;
using EventManagementSystem.BLL.Services.Interfaces;
using EventManagementSystem.BLL.ViewModels.Organizer;
using EventManagementSystem.BLL.ViewModels.User;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace EventManagementSystem.ServerUI.Controllers
{
    public class OrganizersController : Controller
    {
        private readonly IOrganizerService _organizerService;
        private readonly IUserService _userService;

        public OrganizersController(IOrganizerService organizerService, IUserService userService)
        {
            _organizerService = organizerService;
            _userService = userService;
        }

        [HttpGet]
        public async Task<IActionResult> Index(string searchTerm)
        {
            IEnumerable<OrganizerListVM> organizers;

            if (!string.IsNullOrWhiteSpace(searchTerm))
            {
                organizers = await _organizerService.SearchAsync(searchTerm);
                ViewData["SearchTerm"] = searchTerm;
            }
            else
            {
                organizers = await _organizerService.GetAllAsync();
            }

            if (organizers == null || !organizers.Any())
            {
                TempData[AlertHelper.Error] = "No organizers found.";
                return View(Enumerable.Empty<OrganizerListVM>());
            }
            return View(organizers);
        }

        [HttpGet]
        public async Task<IActionResult> Details(int id)
        {
            var organizer = await _organizerService.GetByIdAsync(id); // Use GetByIdAsync which returns OrganizerDetailsVM
            if (organizer == null)
            {
                TempData[AlertHelper.Error] = "Organizer not found.";
                return NotFound(); // Return NotFound for better HTTP semantics
            }
            return View(organizer);
        }

        [HttpGet]
        public async Task<IActionResult> Create()
        {
            await PopulateAppUsersDropdown(); // Populate dropdown for associated AppUser
            return View(new OrganizerCreateVM()); // Pass an empty VM to the view
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(OrganizerCreateVM viewModel)
        {
            if (!ModelState.IsValid)
            {
                TempData[AlertHelper.Error] = "Please correct the errors in the form and try again.";
                await PopulateAppUsersDropdown(); // Repopulate dropdown on validation failure
                return View(viewModel);
            }

            try
            {
                var createdOrganizer = await _organizerService.AddAsync(viewModel); // AddAsync now returns OrganizerDetailsVM?
                if (createdOrganizer == null)
                {
                    TempData[AlertHelper.Error] = "Failed to create organizer. An unexpected error occurred, or the associated user is already an organizer. Please try again.";
                    await PopulateAppUsersDropdown();
                    return View(viewModel);
                }
                TempData["Success"] = "Organizer created successfully."; // Use "Success" for consistency
                return RedirectToAction(nameof(Index));
            }
            catch (InvalidOperationException ex) // Catch specific exceptions from the service layer, e.g., duplicate AppUserId
            {
                TempData[AlertHelper.Error] = $"Failed to create organizer: {ex.Message}";
                await PopulateAppUsersDropdown();
                return View(viewModel);
            }
            catch (Exception) // Catch any other unexpected errors
            {
                TempData[AlertHelper.Error] = "An unexpected error occurred while creating the organizer. Please try again.";
                await PopulateAppUsersDropdown();
                return View(viewModel);
            }
        }

        [HttpGet]
        public async Task<IActionResult> Update(int id)
        {
            var organizerUpdate = await _organizerService.GetUpdateByIdAsync(id); // Use GetUpdateByIdAsync which returns OrganizerUpdateVM
            if (organizerUpdate == null)
            {
                TempData[AlertHelper.Error] = "Organizer not found for update.";
                return NotFound();
            }
            await PopulateAppUsersDropdown(); // Populate dropdown for associated AppUser
            return View(organizerUpdate);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Update(OrganizerUpdateVM viewModel)
        {
            if (!ModelState.IsValid)
            {
                TempData[AlertHelper.Error] = "Please correct the errors in the form and try again.";
                await PopulateAppUsersDropdown();
                return View(viewModel);
            }

            try
            {
                var updatedOrganizer = await _organizerService.UpdateAsync(viewModel); // UpdateAsync now returns OrganizerDetailsVM?
                if (updatedOrganizer == null)
                {
                    TempData[AlertHelper.Error] = "Failed to update organizer. The organizer might not exist, or the associated user is already an organizer. Please try again.";
                    await PopulateAppUsersDropdown();
                    return View(viewModel);
                }
                TempData[AlertHelper.Success] = "Organizer updated successfully.";
                return RedirectToAction(nameof(Index));
            }
            catch (InvalidOperationException ex)
            {
                TempData[AlertHelper.Error] = $"Failed to update organizer: {ex.Message}";
                await PopulateAppUsersDropdown();
                return View(viewModel);
            }
            catch (Exception)
            {
                TempData[AlertHelper.Error] = "An unexpected error occurred while updating the organizer. Please try again.";
                await PopulateAppUsersDropdown();
                return View(viewModel);
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id)
        {
            var deleteResult = await _organizerService.DeleteAsync(id); // Returns bool
            if (!deleteResult)
            {
                TempData[AlertHelper.Error] = "Failed to delete organizer. Please try again later.";
            }
            else
            {
                TempData["Success"] = "Organizer deleted successfully.";
            }
            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteSelected([FromBody] List<int> ids)
        {
            if (ids == null || !ids.Any())
            {
                return Json(new { success = false, message = "No organizers selected for deletion." });
            }

            int deletedCount = 0;
            foreach (var id in ids)
            {
                if (await _organizerService.DeleteAsync(id))
                {
                    deletedCount++;
                }
            }

            if (deletedCount == ids.Count)
            {
                return Json(new { success = true, message = $"{deletedCount} organizer(s) deleted successfully." });
            }
            else if (deletedCount > 0)
            {
                return Json(new { success = true, message = $"Deleted {deletedCount} out of {ids.Count} organizer(s). Some failed." });
            }
            else
            {
                return Json(new { success = false, message = "Failed to delete any selected organizers." });
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Approve(int id)
        {
            var result = await _organizerService.ApproveOrganizerAsync(id); // Returns bool
            if (result)
            {
                TempData["Success"] = "Organizer approved successfully.";
            }
            else
            {
                TempData[AlertHelper.Error] = "Failed to approve organizer. Please try again.";
            }
            return RedirectToAction(nameof(Index));
        }

        /// <summary>
        /// Populates ViewData with SelectList items for the AppUser dropdown.
        /// </summary>
        private async Task PopulateAppUsersDropdown()
        {
            var appUsers = (await _userService.GetAllAsync()) ?? Enumerable.Empty<UserListVM>();

            // Filter out any null UserListVM objects AND users where Id or FullName are null or empty
            var filteredAppUsers = appUsers.Where(u => u != null && !string.IsNullOrEmpty(u.Id) && !string.IsNullOrEmpty(u.FullName)).ToList();

            // Corrected: Use "FullName" as the display text field for the SelectList
            ViewData["Users"] = new SelectList(filteredAppUsers, "Id", "FullName");
        }
    }
}
