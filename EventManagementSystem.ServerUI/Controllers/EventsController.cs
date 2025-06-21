using EventManagementSystem.BLL.Infrastructure;
using EventManagementSystem.BLL.Services.Interfaces;
using EventManagementSystem.BLL.ViewModels.Event;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace EventManagementSystem.ServerUI.Controllers
{
    public class EventsController : Controller
    {
        private readonly IEventService _eventService;
        private readonly IEventTypeService _eventTypeService; // Assuming you have a service for EventType
        private readonly ILocationService _locationService; // Assuming you have a service for Location
        private readonly IOrganizerService _organizerService; // Assuming you have a service for Organizer

        public EventsController(IEventService eventService,
                                IEventTypeService eventTypeService,
                                ILocationService locationService,
                                IOrganizerService organizerService)
        {
            _eventService = eventService;
            _eventTypeService = eventTypeService;
            _locationService = locationService;
            _organizerService = organizerService;
        }

        [HttpGet]
        public async Task<IActionResult> Index(string searchTerm)
        {
            IEnumerable<EventListVM> events;

            if (!string.IsNullOrWhiteSpace(searchTerm))
            {
                events = await _eventService.SearchEventsAsync(searchTerm);
                ViewData["SearchTerm"] = searchTerm; // Pass search term back to the view
            }
            else
            {
                events = await _eventService.GetAllAsync();
            }

            if (events == null || !events.Any())
            {
                TempData[AlertHelper.Error] = "No events found.";
                return View(Enumerable.Empty<EventListVM>()); // Return empty list instead of new List<>() for consistency
            }
            return View(events);
        }

        [HttpGet]
        public async Task<IActionResult> Details(int id)
        {
            var eventDetails = await _eventService.GetByIdAsync(id);
            if (eventDetails == null)
            {
                TempData[AlertHelper.Error] = "Event not found.";
                return NotFound();
            }
            return View(eventDetails);
        }

        [HttpGet]
        public async Task<IActionResult> Create()
        {
            await PopulateDropdowns(); // Populate dropdowns for EventType, Location, Organizer
            return View(new EventCreateVM()); // Pass a new, empty view model to the view
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(EventCreateVM viewModel)
        {
            // Clear custom errors related to IFormFile before ModelState.IsValid check
            // This is a common pattern when IFormFile is required, but an empty form might cause errors.
            // However, our VM already uses [Required] for PhotoFile, so basic validation should cover.
            // Additional custom validation for file types/sizes can be done in service or custom validator.

            if (!ModelState.IsValid)
            {
                TempData[AlertHelper.Error] = "Please correct the errors in the form and try again.";
                await PopulateDropdowns(); // Repopulate dropdowns on validation failure
                return View(viewModel);
            }

            try
            {
                var createdEvent = await _eventService.AddAsync(viewModel);
                if (createdEvent == null)
                {
                    TempData[AlertHelper.Error] = "Failed to create event. An unexpected error occurred. Please try again.";
                    await PopulateDropdowns(); // Repopulate dropdowns on service failure
                    return View(viewModel);
                }
                TempData["Success"] = "Event created successfully.";
                return RedirectToAction(nameof(Index));
            }
            catch (InvalidOperationException ex) // Catch specific exceptions from the service layer
            {
                TempData[AlertHelper.Error] = $"Failed to create event: {ex.Message}";
                await PopulateDropdowns();
                return View(viewModel);
            }
            catch (Exception) // Catch any other unexpected errors
            {
                TempData[AlertHelper.Error] = "An unexpected error occurred while creating the event. Please try again.";
                await PopulateDropdowns();
                return View(viewModel);
            }
        }

        [HttpGet]
        public async Task<IActionResult> Update(int id)
        {
            var eventUpdate = await _eventService.GetUpdateByIdAsync(id);
            if (eventUpdate == null)
            {
                TempData[AlertHelper.Error] = "Event not found for update.";
                return NotFound();
            }
            await PopulateDropdowns(); // Populate dropdowns for EventType, Location, Organizer
            return View(eventUpdate);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Update(EventUpdateVM viewModel)
        {
            if (!ModelState.IsValid)
            {
                TempData[AlertHelper.Error] = "Please correct the errors in the form and try again.";
                await PopulateDropdowns(); // Repopulate dropdowns on validation failure
                return View(viewModel);
            }

            try
            {
                var updatedEvent = await _eventService.UpdateAsync(viewModel);
                if (updatedEvent == null)
                {
                    TempData[AlertHelper.Error] = "Failed to update event. An unexpected error occurred. Please try again.";
                    await PopulateDropdowns(); // Repopulate dropdowns on service failure
                    return View(viewModel);
                }
                TempData["Success"] = "Event updated successfully.";
                return RedirectToAction(nameof(Index));
            }
            catch (InvalidOperationException ex) // Catch specific exceptions from the service layer
            {
                TempData[AlertHelper.Error] = $"Failed to update event: {ex.Message}";
                await PopulateDropdowns();
                return View(viewModel);
            }
            catch (Exception) // Catch any other unexpected errors
            {
                TempData[AlertHelper.Error] = "An unexpected error occurred while updating the event. Please try again.";
                await PopulateDropdowns();
                return View(viewModel);
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id)
        {
            var deleteResult = await _eventService.DeleteAsync(id);
            if (!deleteResult)
            {
                TempData[AlertHelper.Error] = "Failed to delete event. Please try again later.";
            }
            else
            {
                TempData["Success"] = "Event deleted successfully.";
            }
            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteSelected([FromBody] List<int> ids)
        {
            if (ids == null || !ids.Any())
            {
                return Json(new { success = false, message = "No events selected for deletion." });
            }

            int deletedCount = 0;
            foreach (var id in ids)
            {
                if (await _eventService.DeleteAsync(id))
                {
                    deletedCount++;
                }
            }

            if (deletedCount == ids.Count)
            {
                return Json(new { success = true, message = $"{deletedCount} event(s) deleted successfully." });
            }
            else if (deletedCount > 0)
            {
                return Json(new { success = true, message = $"Deleted {deletedCount} out of {ids.Count} event(s). Some failed." });
            }
            else
            {
                return Json(new { success = false, message = "Failed to delete any selected events." });
            }
        }

        /// <summary>
        /// Populates ViewData with SelectList items for EventType, Location, and Organizer dropdowns.
        /// </summary>
        private async Task PopulateDropdowns()
        {
            // Fetch all event types and create a SelectList
            var eventTypes = await _eventTypeService.GetAllAsync(); // Assuming this returns IEnumerable<EventTypeListVM> or similar
            ViewData["EventTypes"] = new SelectList(eventTypes, "Id", "Name");

            // Fetch all locations and create a SelectList
            var locations = await _locationService.GetAllAsync(); // Assuming this returns IEnumerable<LocationListVM> or similar
            ViewData["Locations"] = new SelectList(locations, "Id", "Name");

            // Fetch all organizers and create a SelectList
            // Note: You might need a specific OrganizerListVM that includes the display name
            var organizers = await _organizerService.GetAllAsync(); // Assuming this returns IEnumerable<OrganizerListVM> or similar
            ViewData["Organizers"] = new SelectList(organizers, "Id", "Name"); // Adjust "Name" to whatever property holds the organizer's display name
        }
    }
}
