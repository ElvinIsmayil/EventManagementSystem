using EventManagementSystem.BLL.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace EventManagementSystem.ClientUI.Controllers
{
    public class EventsController : Controller
    {
        private readonly IEventService _eventService;

        public EventsController(IEventService eventService)
        {
            _eventService = eventService;
        }

        public async Task<IActionResult> Index()
        {
            var events = await _eventService.GetAllAsync();
            return View(events);
        }

        [HttpGet]
        public async Task<IActionResult> Details(int id)
        {
            var eventDetails = await _eventService.GetByIdAsync(id);
            if (eventDetails == null)
            {
                TempData["Error"] = "Event not found.";
                return NotFound();
            }
            return View(eventDetails);
        }


    }
}
