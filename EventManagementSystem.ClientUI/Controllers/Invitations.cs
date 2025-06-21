using Microsoft.AspNetCore.Mvc;

namespace EventManagementSystem.ClientUI.Controllers
{
    public class Invitations : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
