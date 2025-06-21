using EventManagementSystem.BLL.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace EventManagementSystem.ClientUI.Controllers
{
    public class EventTypesController : Controller
    {
        private readonly IEventTypeService _eventTypeService;

        public EventTypesController(IEventTypeService eventTypeService)
        {
            _eventTypeService = eventTypeService;
        }

        public async Task<IActionResult> Details(int id)
        {
            var eventType = await _eventTypeService.GetByIdAsync(id);
            if (eventType == null)
            {
                TempData["Error"] = "Event Type not found.";
                return NotFound();
            }
            return View(eventType);
        }

    }
}
