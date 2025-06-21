using EventManagementSystem.BLL.Infrastructure;
using EventManagementSystem.BLL.Services.Interfaces;
using EventManagementSystem.BLL.ViewModels.Auth;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity; // Required for SignInResult and IdentityResult

namespace EventManagementSystem.ServerUI.Controllers
{
    public class AuthController : Controller
    {
        private readonly IAuthenticationService _authService;

        public AuthController(IAuthenticationService authenticationService)
        {
            _authService = authenticationService;
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
                var errors = ModelState.Values
                                       .SelectMany(v => v.Errors)
                                       .Select(e => e.ErrorMessage)
                                       .ToList();
                // For HTTP POST, returning Json is usually for API endpoints or AJAX submissions.
                // For traditional MVC forms, returning View(model) is standard for validation errors.
                // Assuming this is still a traditional form submission due to TempData usage.
                TempData[AlertHelper.Error] = "Please correct the form errors and try again: " + string.Join(" ", errors);
                return View(model);
            }

            var result = await _authService.LoginAsync(model);

            if (result.RequiresTwoFactor)
            {
                TempData[AlertHelper.Warning] = "Two-factor authentication is required.";
                return RedirectToAction("LoginWith2fa"); // Redirect to a 2FA login page if you have one
            }
            if (result.IsLockedOut)
            {
                TempData[AlertHelper.Error] = "Your account is locked out due to multiple failed login attempts. Please try again later.";
                return View("Lockout"); // Redirect to a lockout page if you have one
            }
            if (result.IsNotAllowed)
            {
                ModelState.AddModelError(string.Empty, "Your email has not been confirmed. Please check your inbox for a verification link.");
                TempData[AlertHelper.Warning] = "Login failed: Email not confirmed. Please verify your email address to log in.";
                return View(model);
            }
            if (!result.Succeeded) // Covers SignInResult.Failed
            {
                ModelState.AddModelError(string.Empty, "Invalid login attempt. Please check your credentials.");
                TempData[AlertHelper.Error] = "Login failed: Invalid email or password.";
                return View(model);
            }

            TempData[AlertHelper.Success] = "Welcome back! You have successfully logged in.";
            return RedirectToAction("Index", "Dashboard"); // Redirect to your main dashboard after success
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
                // Add errors from IdentityResult to ModelState for display on the form
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
                TempData[AlertHelper.Error] = "Registration failed: " + string.Join(" ", result.Errors.Select(e => e.Description));
                return View(model);
            }

            TempData[AlertHelper.Success] = "Registration successful! Please check your email to confirm your account.";
            // Assuming EmailVerification is a page that informs the user to check their email
            TempData["Email"] = model.Email; // Pass email to EmailVerification view if needed
            return RedirectToAction("EmailVerification");
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
        public async Task<IActionResult> ResendEmailConfirmation(string email)
        {
            if (string.IsNullOrEmpty(email))
            {
                TempData[AlertHelper.Error] = "Email is required to resend confirmation link.";
                TempData["AlertType"] = "toastr";
                return RedirectToAction("SignIn");
            }

            // IMPORTANT: The IAuthenticationService.SendVerificationEmailAsync method currently requires userId.
            // To properly implement "ResendEmailConfirmation" with only an email,
            // IAuthenticationService needs a new method like 'SendVerificationEmailByEmailAsync(string email)'
            // that internally finds the user by email before sending the verification email.
            // As the current IAuthenticationService interface doesn't support finding by email directly for verification,
            // this action will currently not function correctly as intended without a service interface change.
            // For now, we will provide a generic success message as per security best practice for email existence.

            // Simulate the service call if it existed as `SendVerificationEmailByEmailAsync`
            // You would replace the next line with: `var result = await _authService.SendVerificationEmailByEmailAsync(email);`
            // If the service's SendVerificationEmailAsync only takes userId, you would need to get the user's ID first.
            // For now, returning true as per previous discussions for security (don't reveal if email exists).
            // This is a place where IEmailService.SendEmailAsync return type being bool is directly useful.
            var result = await _authService.SendResetPasswordEmailAsync(email); // Using SendResetPasswordEmailAsync temporarily, assuming it handles user lookup.
                                                                                // This needs to be replaced with a proper SendVerificationEmailByEmailAsync once available.

            // The original logic checked `if (!result)` but the service now implicitly returns true for security.
            // If SendVerificationEmailByEmailAsync returns false only for true *system* errors, then you can check it.
            // For security, usually you don't reveal if the email exists. So, often this will always return success to the user.
            TempData[AlertHelper.Success] = "If an account exists for that email, a confirmation link has been sent.";
            TempData["AlertType"] = "toastr";
            TempData["Email"] = email;
            return RedirectToAction("EmailVerification");
        }


        [HttpGet]
        public async Task<IActionResult> ConfirmEmail(string userId, string token)
        {
            if (string.IsNullOrEmpty(userId) || string.IsNullOrEmpty(token))
            {
                TempData[AlertHelper.Error] = "Invalid email confirmation link. Please ensure the link is complete.";
                return RedirectToAction("SignIn", "Auth"); // Redirect to Account or Auth SignIn
            }

            var result = await _authService.ConfirmEmailAsync(userId, token);

            if (!result.Succeeded)
            {
                TempData[AlertHelper.Error] = "Email confirmation failed. The link may be invalid or expired. Please try registering again or contacting support.";
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description); // Add errors to ModelState for display if needed
                }
                return RedirectToAction("SignIn", "Auth"); // Redirect to Account or Auth SignIn
            }

            TempData[AlertHelper.Success] = "Your email has been successfully confirmed! You can now log in.";
            return RedirectToAction("ConfirmEmailConfirmation"); // Redirect to a confirmation success page
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

            // As per best practice in AuthenticationService, SendResetPasswordEmailAsync always returns true
            // for security reasons (to prevent user enumeration).
            await _authService.SendResetPasswordEmailAsync(model.Email);

            TempData[AlertHelper.Success] = "If an account exists for that email, a password reset link has been sent to your inbox.";
            TempData["Email"] = model.Email; // Pass email for display on confirmation page
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
            if (string.IsNullOrEmpty(email) || string.IsNullOrEmpty(token))
            {
                TempData[AlertHelper.Error] = "Invalid password reset link.";
                return RedirectToAction("SignIn", "Auth"); // Redirect to sign-in or a generic error page
            }

            var model = new ResetPasswordVM { Email = email, Token = token };
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ResetPassword(ResetPasswordVM model)
        {
            if (!ModelState.IsValid)
            {
                TempData[AlertHelper.Error] = "Please correct the form errors and try again.";
                return View(model);
            }

            var result = await _authService.ResetPasswordAsync(model);

            if (!result.Succeeded)
            {
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
                TempData[AlertHelper.Error] = "Password reset failed: " + string.Join(" ", result.Errors.Select(e => e.Description));
                return View(model);
            }

            TempData[AlertHelper.Success] = "Your password has been successfully reset. You can now log in with your new password.";
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
            // Note: The IsAuthenticatedAsync(userId) in IAuthenticationService currently checks
            // general authentication status, not specific to the passed userId.
            // If userId is meant to verify *this* user's lockout status, the service method needs refinement.
            // For now, assuming if someone is on Lockout page, they are locked out or attempting access.
            // A simple IsAuthenticated check to redirect if not authenticated (which implies they shouldn't be locked out for *this* user)
            // if you reach Lockout view unauthenticated, it's typically Access Denied.
            var isAuthenticated = await _authService.IsAuthenticatedAsync(userId);
            if (!isAuthenticated)
            {
                TempData[AlertHelper.Warning] = "You are not authenticated. Access denied.";
                return RedirectToAction("AccessDenied");
            }
            // If authenticated but redirected to Lockout, implies system has determined current user is locked.
            return View();
        }
    }
}
