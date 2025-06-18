using EventManagementSystem.BLL.Services.Interfaces;
using EventManagementSystem.BLL.ViewModels.Location;
using Microsoft.AspNetCore.Mvc;

namespace EventManagementSystem.ServerUI.ServerUI.Controllers
{
    public class LocationsController : Controller
    {
        private readonly ILocationService _locationsService;

        public LocationsController(ILocationService locationsService)
        {
            _locationsService = locationsService;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            try
            {
                var locations = await _locationsService.GetAllLocationsWithPhotosAsync();
                return View(locations);
            }
            catch (Exception ex)
            {
                TempData["Error"] = "An error occurred while loading locations. Please try again later.";
                return View(new List<LocationListVM>());
            }
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(LocationCreateVM viewModel)
        {
            if (!ModelState.IsValid)
            {
                TempData["Error"] = "Please correct the errors in the form and try again.";
                return View(viewModel);
            }
            var createdLocation = await _locationsService.AddAsync(viewModel);
            if (createdLocation == null)
            {
                TempData["Error"] = "Failed to create location. Please check your input and try again.";
                return View(viewModel);
            }
            TempData["Success"] = "Location created successfully.";
            return RedirectToAction(nameof(Index));

        }

        [HttpGet]
        public async Task<IActionResult> Update(int id)
        {
            var location = await _locationsService.GetUpdateByIdAsync(id);
            if (location == null)
            {
                TempData["Error"] = "Location not found.";
                return RedirectToAction(nameof(Index));
            }
            return View(location);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Update(LocationUpdateVM viewModel)
        {
            if (!ModelState.IsValid)
            {
                TempData["Error"] = "Please correct the errors in the form and try again.";
                return View(viewModel);
            }
            var updatedLocation = await _locationsService.UpdateAsync(viewModel);
            if (updatedLocation == null)
            {
                TempData["Error"] = "Failed to update location. Please check your input and try again.";
                return View(viewModel);
            }
            TempData["Success"] = "Location updated successfully.";
            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteSelected([FromBody] List<int> ids)
        {
            if (ids == null || !ids.Any())
            {
                return Json(new { success = false, message = "No event types selected for deletion." });
            }

            var (deletedCount, failedDeletions) = await _locationsService.DeleteMultipleAsync(ids);

            if (deletedCount == ids.Count)
            {
                return Json(new { success = true, message = $"{deletedCount} event type(s) deleted successfully." });
            }
            else if (deletedCount > 0)
            {
                return Json(new { success = true, message = $"{deletedCount} event type(s) deleted successfully. Some failed: {string.Join("; ", failedDeletions)}" });
            }
            else
            {
                return Json(new { success = false, message = $"Failed to delete any event types. Errors: {string.Join("; ", failedDeletions)}" });
            }
        }

        [HttpGet]
        public async Task<IActionResult> Details(int id)
        {
            var location = await _locationsService.GetByIdAsync(id);
            if (location == null)
            {
                TempData["Error"] = "Location not found.";
                return RedirectToAction(nameof(Index));
            }
            return View(location);
        }


    }
}