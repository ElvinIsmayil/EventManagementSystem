using EventManagementSystem.BLL.Infrastructure.Interfaces;
using EventManagementSystem.DAL.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using IAuthenticationService = EventManagementSystem.BLL.Services.Interfaces.IAuthenticationService;

namespace EventManagementSystem.BLL.Services.Implementations
{
    public class AuthenticationService : IAuthenticationService
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly SignInManager<AppUser> _signInManager;
        private readonly IEmailService _emailService;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly IHttpContextAccessor _httpContextAccessor;
        public AuthenticationService(UserManager<AppUser> userManager, SignInManager<AppUser> signInManager, IEmailService emailService, RoleManager<IdentityRole> roleManager, IHttpContextAccessor httpContextAccessor)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _emailService = emailService;
            _roleManager = roleManager;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<bool> RegisterAsync(string email, string password, string confirmPassword)
        {
            if (password != confirmPassword) return false;
            if (await _userManager.FindByEmailAsync(email) != null) return false;

            var user = new AppUser { UserName = email, Email = email };
            var result = await _userManager.CreateAsync(user, password);

            if (!result.Succeeded) return false;
            if (!await _roleManager.RoleExistsAsync("User"))
                await _roleManager.CreateAsync(new IdentityRole("User"));

            await _userManager.AddToRoleAsync(user, "User");
            return await SendVerificationEmailAsync(user.Id, email);
        }
        public async Task<bool> LoginAsync(string email, string password, bool rememberMe)
        {
            var user = await _userManager.FindByEmailAsync(email);
            if (user == null || !await _userManager.IsEmailConfirmedAsync(user))
                return false;

            var result = await _signInManager.PasswordSignInAsync(email, password, rememberMe, lockoutOnFailure: false);
            return result.Succeeded;
        }
        public async Task LogoutAsync()
        {
            await _signInManager.SignOutAsync();
        }
        public async Task<bool> IsAuthenticatedAsync(string userId)
        {
            return (_httpContextAccessor.HttpContext?.User?.Identity?.IsAuthenticated ?? false);
        }
        public async Task<string> GetCurrentUserIdAsync()
        {
            var userPrincipal = _httpContextAccessor.HttpContext?.User;
            if (userPrincipal == null)
                return null;

            return _userManager.GetUserId(userPrincipal);
        }

        public async Task<string> GetCurrentUserEmailAsync()
        {
            var user = await _userManager.GetUserAsync(_httpContextAccessor.HttpContext?.User);
            return user?.Email;
        }
        public async Task<bool> ChangePasswordAsync(string currentPassword, string newPassword, string confirmNewPassword)
        {
            if (newPassword != confirmNewPassword)
                return false;

            var user = await _userManager.GetUserAsync(_httpContextAccessor.HttpContext?.User);
            if (user == null)
                return false; // No authenticated user to change password for

            var result = await _userManager.ChangePasswordAsync(user, currentPassword, newPassword);
            return result.Succeeded;
        }

        public async Task<bool> SendResetPasswordEmailAsync(string email)
        {
            var user = await _userManager.FindByEmailAsync(email);
            if (user == null || !(await _userManager.IsEmailConfirmedAsync(user)))
                return false; // Ensure the user exists and email is confirmed

            var token = await _userManager.GeneratePasswordResetTokenAsync(user);
            var resetLink = $"{_httpContextAccessor.HttpContext?.Request.Scheme}://{_httpContextAccessor.HttpContext?.Request.Host}/Account/ResetPassword?userId={user.Id}&token={Uri.EscapeDataString(token)}";

            await _emailService.SendEmailAsync(email, "Reset Your Password", $"Click the link to reset your password: {resetLink}");

            return true; // Email sent successfully
        }

        public async Task<bool> ConfirmEmailAsync(string userId, string token)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
                return false;

            var result = await _userManager.ConfirmEmailAsync(user, token);
            return result.Succeeded;
        }

        public async Task<bool> SendVerificationEmailAsync(string userId, string email)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null) return false;

            var token = await _userManager.GenerateEmailConfirmationTokenAsync(user);
            var verificationLink = $"{_httpContextAccessor.HttpContext?.Request.Scheme}://{_httpContextAccessor.HttpContext?.Request.Host}/Account/ConfirmEmail?userId={userId}&token={Uri.EscapeDataString(token)}";

            await _emailService.SendEmailAsync(email, "Confirm Your Email", $"Click this link to verify your email: {verificationLink}");
            return true;
        }
        public async Task<bool> ResetPasswordAsync(string userId, string token, string newPassword, string confirmNewPassword)
        {
            if (newPassword != confirmNewPassword) return false;

            var user = await _userManager.FindByIdAsync(userId);
            if (user == null) return false;

            var result = await _userManager.ResetPasswordAsync(user, token, newPassword);
            return result.Succeeded;
        }


    }
}

