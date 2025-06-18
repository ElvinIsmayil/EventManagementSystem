using EventManagementSystem.BLL.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace EventManagementSystem.ClientUI.Controllers
{
    public class HomeController : Controller
    {
        private readonly IEventTypeService _eventTypeService;

        public HomeController(IEventTypeService eventTypeService)
        {
            _eventTypeService = eventTypeService;
        }

        public async Task<IActionResult> Index()
        {
            var eventTypes = await _eventTypeService.GetAllAsync();
            return View(eventTypes);
        }

    }
}
