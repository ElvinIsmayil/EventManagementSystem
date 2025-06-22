using EventManagementSystem.BLL.Services.Interfaces;
using EventManagementSystem.DAL.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using System.Threading.Tasks;

namespace EventManagementSystem.ClientUI.Controllers
{
    [Authorize]
    public class OrganizersController : Controller
    {
        private readonly IEventService _eventService;
        private readonly IInvitationService _invitationService;
        private readonly UserManager<AppUser> _userManager;

        public OrganizersController(IEventService eventService, UserManager<AppUser> userManager, IInvitationService invitationService)
        {
            _eventService = eventService;
            _userManager = userManager;
            _invitationService = invitationService;
        }

        public async Task<IActionResult> Index()
        {
            string userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var user = await _userManager.Users.Include(x=>x.Organizer).FirstOrDefaultAsync(x=>x.Id==userId);
            if (user == null)
            {
                return RedirectToAction("SignIn", "Auth");
            }

            var events = await _eventService.GetEventsByOrganizerIdAsync(user.Organizer.Id);
            return View(events);
        }

        public async Task<IActionResult> SendInvitationToStudents(int eventId)
        {
            var eventEntity = await _eventService.GetByIdAsync(eventId);
            if (eventEntity == null)
            {
                TempData["ErrorMessage"] = "Event not found.";
                return RedirectToAction("Index");
            }
            await _invitationService.SendInvitationAsync(eventId);
            TempData["SuccessMessage"] = "Invitation sent successfully to all students.";
            return RedirectToAction("Index");


        }


    }
}
