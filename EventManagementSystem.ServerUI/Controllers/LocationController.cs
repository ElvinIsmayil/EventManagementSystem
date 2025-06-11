using EventManagementSystem.BLL.Services.Interfaces;
using EventManagementSystem.BLL.ViewModels.Location;
using Microsoft.AspNetCore.Mvc;

namespace EventManagementSystem.ServerUI.Controllers
{
    public class LocationController : Controller
    {
        private readonly ILocationService _locationService;
        private readonly ILocationPhotoService _locationPhotoService;

        public LocationController(ILocationService locationService, ILocationPhotoService locationPhotoService)
        {
            _locationService = locationService;
            _locationPhotoService = locationPhotoService;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            try
            {
                var locations = await _locationService.GetAllLocationsWithPhotosAsync();
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
            var createdLocation = await _locationService.AddAsync(viewModel);
            if (createdLocation == null)
            {
                TempData["Error"] = "Failed to create location. Please check your input and try again.";
                return View(viewModel);
            }
            TempData["Success"] = "Location created successfully.";
            return RedirectToAction(nameof(Index));

        }

        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var location = await _locationService.GetUpdateByIdAsync(id);
            if (location == null)
            {
                TempData["Error"] = "Location not found.";
                return RedirectToAction(nameof(Index));
            }
            return View(location);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(LocationUpdateVM viewModel)
        {
            if (!ModelState.IsValid)
            {
                TempData["Error"] = "Please correct the errors in the form and try again.";
                return View(viewModel);
            }
            var updatedLocation = await _locationService.UpdateAsync(viewModel);
            if (updatedLocation == null)
            {
                TempData["Error"] = "Failed to update location. Please check your input and try again.";
                return View(viewModel);
            }
            TempData["Success"] = "Location updated successfully.";
            return RedirectToAction(nameof(Index));
        }

    }
}
