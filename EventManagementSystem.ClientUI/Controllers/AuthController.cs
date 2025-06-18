using EventManagementSystem.BLL.Services.Interfaces;
using EventManagementSystem.BLL.ViewModels.Auth;
using Microsoft.AspNetCore.Mvc;

namespace EventManagementSystem.ClientUI.Controllers
{
    public class AuthController : Controller
    {
        private readonly IAuthenticationService _authService;

        public AuthController(IAuthenticationService authService)
        {
            _authService = authService;
        }

        [HttpGet]
        public IActionResult SignIn()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> SignIn(SignInVM model)
        {
            if (!ModelState.IsValid)
            {
                TempData["ToastrMessage"] = "Please correct the errors in the form.";
                TempData["ToastrType"] = "warning";
                return View(model);
            }

            var result = await _authService.LoginAsync(model);

            if (result)
            {
                TempData["ToastrMessage"] = "Welcome back! You have been successfully logged in.";
                TempData["ToastrType"] = "success";
                return RedirectToAction("Index", "Home");
            }
            else
            {
                ModelState.AddModelError(string.Empty, "Invalid login attempt. Please check your credentials.");

                TempData["SwalMessage"] = "The email or password you entered is incorrect.";
                TempData["SwalType"] = "error";
                TempData["SwalTitle"] = "Login Failed";

                return View(model);
            }
        }

        [HttpGet]
        public IActionResult SignUp()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> SignUp(SignUpVM model)
        {
            if (!ModelState.IsValid)
            {
                TempData["ToastrMessage"] = "Please correct the form errors and try again.";
                TempData["ToastrType"] = "warning";
                return View(model);
            }

            var result = await _authService.RegisterAsync(model);

            if (result)
            {
                TempData["ToastrMessage"] = "Registration successful! You can now sign in with your new account.";
                TempData["ToastrType"] = "success";
                return RedirectToAction("SignIn", "Auth");
            }
            else
            {
                ModelState.AddModelError(string.Empty, "Registration failed. Please try again or use a different email.");

                TempData["SwalMessage"] = "Account creation failed. This email might already be in use or there was a server error.";
                TempData["SwalType"] = "error";
                TempData["SwalTitle"] = "Registration Error";

                return View(model);
            }
        }

        [HttpGet]
        public IActionResult ResetPassword()
        {
            return View();
        }

        public async Task<IActionResult> SignOut()
        {
            await _authService.LogoutAsync();
            return RedirectToAction("SignIn", "Auth");
        }

        [HttpGet]
        public async Task<IActionResult> ConfirmEmail(string userId, string token)
        {
            if (string.IsNullOrEmpty(userId) || string.IsNullOrEmpty(token))
            {
                TempData["ToastrMessage"] = "Invalid email confirmation link.";
                TempData["ToastrType"] = "error";
                return RedirectToAction("SignIn", "Auth");
            }
            var result = await _authService.ConfirmEmailAsync(userId, token);
            if (result)
            {
                TempData["ToastrMessage"] = "Your email has been successfully confirmed! You can now log in.";
                TempData["ToastrType"] = "success";
                return RedirectToAction("SignIn", "Auth");
            }
            else
            {
                TempData["ToastrMessage"] = "Email confirmation failed. Please try again or contact support.";
                TempData["ToastrType"] = "error";
                return RedirectToAction("SignIn", "Auth");
            }

        }
    }
}
