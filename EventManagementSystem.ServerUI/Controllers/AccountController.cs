using EventManagementSystem.BLL.Services.Interfaces;
using EventManagementSystem.BLL.ViewModels.Account;
using EventManagementSystem.Helpers;
using Microsoft.AspNetCore.Mvc;

namespace EventManagementSystem.ServerUI.Controllers
{
    public class AccountController : Controller
    {
        private readonly IAuthenticationService _authService;

        public AccountController(IAuthenticationService authenticationService)
        {
            _authService = authenticationService;
        }

        [HttpGet]
        public IActionResult SignUp()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> SignUp(SignUpVM signUpVM)
        {
            if (!ModelState.IsValid)
            {
                TempData[AlertHelper.Error] = "Please fill out the form correctly.";
                return View(signUpVM);
            }

            if (!signUpVM.Toc)
            {
                TempData[AlertHelper.Error] = "You must agree to the terms and conditions.";
                return View(signUpVM);
            }

            var result = await _authService.RegisterAsync(signUpVM.Email, signUpVM.Password, signUpVM.ConfirmPassword);
            if (!result)
            {
                TempData[AlertHelper.Error] = "Registration failed. Please check your details and try again.";
                return View(signUpVM);
            }

            var emailSent = await _authService.SendVerificationEmailAsync(signUpVM.Email, signUpVM.Email);
            TempData[AlertHelper.Success] = emailSent
                ? "Registration successful! Please check your email to confirm your account."
                : "Registration successful, but verification email could not be sent.";

            return RedirectToAction("EmailVerification");
        }


        [HttpGet]
        public IActionResult SignIn()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> SignIn(SignInVM signInVM)
        {
            if (!ModelState.IsValid)
            {
                TempData[AlertHelper.Error] = "Please fix the form validation errors.";
                TempData["AlertType"] = "swal";
                return View(signInVM);
            }

            var result = await _authService.LoginAsync(signInVM.Email, signInVM.Password, signInVM.RememberMe);
            if (!result)
            {
                TempData[AlertHelper.Error] = "Invalid email or password.";
                TempData["AlertType"] = "swal";
                return View(signInVM);
            }

            TempData[AlertHelper.Success] = "Welcome back!";
            return RedirectToAction("Index", "Dashboard");
        }

        public async Task<IActionResult> SignOut()
        {
            await _authService.LogoutAsync();
            return RedirectToAction("SignIn", "Account");
        }

        [HttpGet]
        public async Task<IActionResult> ResendEmailConfirmation(string email)
        {
            var result = await _authService.SendVerificationEmailAsync(email, email);
            if (!result)
            {
                TempData[AlertHelper.Error] = "Invalid or already confirmed email.";
                TempData["AlertType"] = "toastr";
                return RedirectToAction("SignIn");
            }

            TempData[AlertHelper.Success] = "Confirmation email resent.";
            TempData["AlertType"] = "toastr";
            return RedirectToAction("EmailVerification");
        }

        [HttpGet]
        public async Task<IActionResult> ConfirmEmail(string userId, string token)
        {
            if (string.IsNullOrEmpty(userId) || string.IsNullOrEmpty(token))
                return RedirectToAction("SignIn", "Account");

            var result = await _authService.ConfirmEmailAsync(userId, token);
            if (!result)
                return View("Error");

            return RedirectToAction("ConfirmEmailConfirmation");
        }

        [HttpGet]
        public IActionResult ConfirmEmailConfirmation()
        {
            return View();
        }

        [HttpGet]
        public IActionResult EmailVerification()
        {
            ViewBag.Email = TempData["Email"] as string;
            return View();
        }

        [HttpGet]
        public IActionResult ForgotPassword()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ForgotPassword(ForgotPasswordVM model)
        {
            if (!ModelState.IsValid)
            {
                TempData[AlertHelper.Error] = "Please enter a valid email.";
                return View(model);
            }

            var result = await _authService.SendResetPasswordEmailAsync(model.Email);
            if (!result)
            {
                TempData["Email"] = model.Email;
                return RedirectToAction("ForgotPasswordConfirmation");
            }

            TempData[AlertHelper.Success] = "Password reset email sent.";
            TempData["Email"] = model.Email;
            return RedirectToAction("ForgotPasswordConfirmation");
        }

        [HttpGet]
        public IActionResult ForgotPasswordConfirmation()
        {
            ViewBag.Email = TempData["Email"] as string;
            return View();
        }

        [HttpGet]
        public IActionResult ResetPassword(string email, string token)
        {
            if (email == null || token == null)
                return RedirectToAction("Index", "Home");

            var model = new ResetPasswordVM { Email = email, Token = token };
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ResetPassword(ResetPasswordVM model)
        {
            if (!ModelState.IsValid) return View(model);

            var result = await _authService.ResetPasswordAsync(model.Email, model.Token, model.Password, model.Password);
            if (!result)
                return View(model);

            return RedirectToAction("ResetPasswordConfirmation");
        }

        [HttpGet]
        public IActionResult ResetPasswordConfirmation()
        {
            return View();
        }

        [HttpGet]
        public IActionResult AccessDenied()
        {
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> Lockout(string userId)
        {
            var isAuthenticated = await _authService.IsAuthenticatedAsync(userId);
            if (!isAuthenticated)
                return RedirectToAction("AccessDenied");

            return View();
        }


    }
}
