using EventManagementSystem.BLL.Services.Interfaces;
using EventManagementSystem.BLL.ViewModels.EventType;
using EventManagementSystem.Helpers;
using Microsoft.AspNetCore.Mvc;

namespace EventManagementSystem.ServerUI.Controllers
{
    public class EventTypesController : Controller
    {
        private readonly IEventTypeService _service;

        public EventTypesController(IEventTypeService service)
        {
            _service = service;
        }

        [HttpGet]
        public async Task<IActionResult> Index(string searchTerm)
        {
            try
            {
                ViewData["SearchTerm"] = searchTerm ?? string.Empty;
                if (!string.IsNullOrEmpty(searchTerm))
                {
                    var filteredEventTypes = await _service.SearchEventTypeAsync(searchTerm);
                    return View(filteredEventTypes);
                }
                var eventTypes = await _service.GetAllAsync();
                return View(eventTypes ?? new List<EventTypeListVM>());
            }
            catch (Exception ex)
            {
                TempData[AlertHelper.Error] = "An error occurred while loading event types. Please try again later.";
                return View(new List<EventTypeListVM>());
            }
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(EventTypeCreateVM viewModel)
        {
            if (!ModelState.IsValid)
            {
                TempData[AlertHelper.Error] = "Please correct the errors in the form and try again.";
                return View(viewModel);
            }
            var createdEventType = await _service.AddAsync(viewModel);
            if (createdEventType == null)
            {
                TempData[AlertHelper.Error] = "Failed to create event type. Please check your input and try again.";
                return View(viewModel);
            }
            TempData[AlertHelper.Success] = "Event type created successfully.";
            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                var eventType = await _service.GetByIdAsync(id);
                if (eventType == null)
                {
                    return Json(new { success = false, message = "Event Type not found or already deleted." });
                }

                var result = await _service.DeleteAsync(id);

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

        [HttpGet]
        public async Task<IActionResult> Update(int id)
        {
            var model = await _service.GetUpdateByIdAsync(id);
            if (model == null)
            {
                TempData[AlertHelper.Error] = "The event type you are trying to update was not found.";
                return RedirectToAction(nameof(Index));
            }
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Update(EventTypeUpdateVM model)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    TempData[AlertHelper.Error] = "Please correct the errors in the form and try again.";
                    return View(model);
                }

                var data = await _service.UpdateAsync(model);

                if (data == null)
                {
                    ModelState.AddModelError(string.Empty, "Failed to update event type. The item might not exist or there was a conflict.");
                    return View(model);
                }

                TempData[AlertHelper.Success] = "Event type updated successfully.";
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, "An unexpected error occurred while updating the event type. Please try again later.");
                return View(model);
            }
        }

        [HttpGet]
        public async Task<IActionResult> Details(int id)
        {
            var eventType = await _service.GetByIdAsync(id);
            if (eventType == null)
            {
                TempData[AlertHelper.Error] = "The requested event type could not be found.";
                return RedirectToAction(nameof(Index));
            }
            return View(eventType);
        }
    }
}