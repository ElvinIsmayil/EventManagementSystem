using EventManagementSystem.BLL.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace EventManagementSystem.ClientUI.Controllers
{
    public class InvitationsController : Controller
    {
        private readonly IInvitationService _invitationService;
        private readonly IPersonService _personService;

        public InvitationsController(IInvitationService invitationService, IPersonService personService)
        {
            _invitationService = invitationService;
            _personService = personService;
        }

        [HttpGet]
        public async Task<IActionResult> MyInvitations()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (string.IsNullOrEmpty(userId))
            {
                return RedirectToAction("SignIn", "Auth");
            }

            int? personId = await _personService.GetPersonIdByAppUserIdAsync(userId);

            if (!personId.HasValue)
            {
                TempData["InfoMessage"] = "Your person profile could not be found. Please ensure your profile is complete.";
                return RedirectToAction("Index", "Home");
            }

            try
            {
                var invitations = await _invitationService.GetPersonInvitationsAsync(personId.Value);
                ViewBag.PersonName = await _personService.GetPersonFullNameAsync(personId.Value) ?? User.Identity?.Name ?? "Guest";
                return View(invitations);
            }
            catch (Exception)
            {
                TempData["ErrorMessage"] = "An error occurred while retrieving your invitations.";
                return RedirectToAction("Error", "Home");
            }
        }

        // Changed invitationCode type to string to match service interface
        [HttpGet]
        public async Task<IActionResult> Respond(string invitationCode)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (string.IsNullOrEmpty(userId))
            {
                return RedirectToAction("SignIn", "Auth");
            }

            int? personId = await _personService.GetPersonIdByAppUserIdAsync(userId);

            if (!personId.HasValue)
            {
                TempData["InfoMessage"] = "Your person profile could not be found. Please ensure your profile is complete.";
                return RedirectToAction("Index", "Home");
            }

            try
            {
                await _invitationService.RespondToInvitationAsync(invitationCode, userId);
                TempData["SuccessMessage"] = "Your response has been recorded successfully.";
            }
            catch (Exception)
            {
                TempData["ErrorMessage"] = "An error occurred while responding to the invitation.";
            }

            return RedirectToAction("MyInvitations");
        }
    }
}
