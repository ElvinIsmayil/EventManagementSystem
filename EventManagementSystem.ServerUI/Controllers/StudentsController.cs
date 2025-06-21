using EventManagementSystem.BLL.Services.Implementations;
using EventManagementSystem.BLL.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace EventManagementSystem.ServerUI.Controllers
{
    public class StudentsController : Controller
    {
        private readonly IStudentService _studentService;

        public StudentsController(IStudentService studentService)
        {
            _studentService = studentService;
        }

        public async Task<IActionResult> Index()
        {
            var students = await _studentService.GetUnverifiedStudents();
            return View(students);
        }

        [HttpPost]
        public async Task<IActionResult> Approve(string studentId)
        {
            var result = await _studentService.ApproveAsync(studentId);
            if (result)
            {
                TempData["Success"] = "Student verified successfully.";
            }
            else
            {
                TempData["Error"] = "Failed to verify student. Please try again.";
            }
            return RedirectToAction("Index");
        }

        [HttpPost]
        public async Task<IActionResult> Delete(string studentId)
        {

            try
            {
                var student = await _studentService.GetByIdAsync(studentId);
                if (student == null)
                {
                    return Json(new { success = false, message = "Event Type not found or already deleted." });
                }

                var result = await _studentService.DeleteAsync(studentId);

                if (!result)
                {
                    return Json(new { success = false, message = "Failed to delete event type. It might be in use or protected." });
                }

                return Json(new { success = true, message = "Event Type deleted successfully." });
            }
            catch (Exception)
            {
                return Json(new { success = false, message = "An unexpected error occurred while deleting the event type. Please try again later." });
            }
        }
    }
}
