using EventManagementSystem.BLL.Services.Interfaces;
using EventManagementSystem.BLL.ViewModels.Home;
using Microsoft.AspNetCore.Mvc;

namespace EventManagementSystem.ClientUI.Controllers
{
    public class HomeController : Controller
    {
        private readonly IEventTypeService _eventTypeService;
        private readonly IEventService _eventService;

        public HomeController(IEventTypeService eventTypeService, IEventService eventService)
        {
            _eventTypeService = eventTypeService;
            _eventService = eventService;
        }

        public async Task<IActionResult> Index()
        {
            var eventTypes = await _eventTypeService.GetAllAsync();
            var events = await _eventService.GetAllAsync();
            var viewModel = new HomeIndexVM
            {
                EventTypes = eventTypes,
                Events = events
            };
            return View(viewModel);
        }

        public IActionResult Error()
        {
            return View();
        }

    }
}
