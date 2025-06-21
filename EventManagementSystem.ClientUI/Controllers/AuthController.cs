using EventManagementSystem.BLL.Infrastructure;
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
                TempData[AlertHelper.Error] = "Please correct the form errors and try again.";
                return View(model);
            }

            var result = await _authService.LoginAsync(model);

            if (result.IsLockedOut)
            {
                TempData[AlertHelper.Error] = "Your account is locked out due to multiple failed login attempts. Please try again later.";
                return View("Lockout"); 
            }
            if (result.IsNotAllowed)
            {
                ModelState.AddModelError(string.Empty, "Either your email has not been confirmed or your student account is not approved");
                TempData[AlertHelper.Warning] = "Either your email has not been confirmed or your student account is not approved";
                return View(model);
            }
            if (!result.Succeeded)
            {
                ModelState.AddModelError(string.Empty, "Invalid login attempt. Please check your credentials.");
                TempData[AlertHelper.Error] = "Login failed: Invalid username or password.";
                return View(model);
            }

            TempData[AlertHelper.Success] = "Welcome back! You have successfully logged in.";
            return RedirectToAction("Index", "Home");
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
                TempData[AlertHelper.Error] = "Please correct the form errors and try again.";
                return View(model);
            }

            var result = await _authService.RegisterAsync(model);

            if (!result.Succeeded)
            {
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
                TempData[AlertHelper.Error] = "Registration failed: " + string.Join(" ", result.Errors.Select(e => e.Description));
                return View(model);
            }

            TempData[AlertHelper.Success] = "Registration successful! Please check your email to confirm your account.";
            return RedirectToAction("SignIn", "Auth");
        }

        [HttpGet]
        public IActionResult ResetPassword()
        {
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> SignOut()
        {
            await _authService.LogoutAsync();
            TempData[AlertHelper.Success] = "You have been successfully logged out.";
            return RedirectToAction("SignIn", "Auth");
        }

        [HttpGet]
        public async Task<IActionResult> ConfirmEmail(string userId, string token)
        {
            if (string.IsNullOrEmpty(userId) || string.IsNullOrEmpty(token))
            {
                TempData[AlertHelper.Error] = "Invalid email confirmation link. Please ensure the link is complete.";
                return RedirectToAction("SignIn", "Auth");
            }

            var result = await _authService.ConfirmEmailAsync(userId, token);

            if (!result.Succeeded)
            {
                TempData[AlertHelper.Error] = "Email confirmation failed. The link may be invalid or expired. Please try registering again or contacting support.";
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description); 
                }
                return RedirectToAction("SignIn", "Auth");
            }

            TempData[AlertHelper.Success] = "Your email has been successfully confirmed! You can now log in.";
            return RedirectToAction("SignIn", "Auth");
        }
    }
}
