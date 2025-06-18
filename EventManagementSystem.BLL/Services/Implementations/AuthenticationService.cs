using EventManagementSystem.BLL.Infrastructure.Interfaces;
using EventManagementSystem.BLL.ViewModels.Auth;
using EventManagementSystem.DAL.Entities;
using EventManagementSystem.ServerUI.Helpers;
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

        public async Task<bool> RegisterAsync(SignUpVM model)
        {
            if (await _userManager.FindByEmailAsync(model.Email) != null) return false;

            var user = new AppUser {
                UserName = model.Email,
                Email = model.Email ,
                Name = model.Name, 
                Surname = model.Surname,
            };
            var result = await _userManager.CreateAsync(user, model.Password);

            if (!result.Succeeded) return false;
            if (!await _roleManager.RoleExistsAsync("User"))
                await _roleManager.CreateAsync(new IdentityRole("User"));

            await _userManager.AddToRoleAsync(user, "User");
            return await SendVerificationEmailAsync(user.Id, model.Email);
        }
        public async Task<bool> LoginAsync(SignInVM model)
        {
            var user = await _userManager.FindByEmailAsync(model.Email);
            if (user == null || !await _userManager.IsEmailConfirmedAsync(user))
                return false;

            var result = await _signInManager.PasswordSignInAsync(user, model.Password, model.RememberMe, lockoutOnFailure: false);
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
                return false; 

            var result = await _userManager.ChangePasswordAsync(user, currentPassword, newPassword);
            return result.Succeeded;
        }

        public async Task<bool> SendResetPasswordEmailAsync(string email)
        {
            var user = await _userManager.FindByEmailAsync(email);
            if (user == null || !(await _userManager.IsEmailConfirmedAsync(user)))
                return false;

            var token = await _userManager.GeneratePasswordResetTokenAsync(user);
            var resetLink = $"{_httpContextAccessor.HttpContext?.Request.Scheme}://{_httpContextAccessor.HttpContext?.Request.Host}/Account/ResetPassword?userId={user.Id}&token={Uri.EscapeDataString(token)}";

            var emailBody = EmailTemplateHelper.GetPasswordResetHtml(resetLink);
            await _emailService.SendEmailAsync(email, "Reset Your Password", emailBody);

            return true; 
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
            var verificationLink = $"{_httpContextAccessor.HttpContext?.Request.Scheme}://{_httpContextAccessor.HttpContext?.Request.Host}/Auth/ConfirmEmail?userId={userId}&token={Uri.EscapeDataString(token)}";

            var emailBody = EmailTemplateHelper.GetEmailConfirmationHtml(verificationLink);

            await _emailService.SendEmailAsync(email, "Confirm Your Email", emailBody);
            return true;
        }

        public async Task<bool> ResetPasswordAsync(ResetPasswordVM model)
        {
            if (model.Password != model.ConfirmPassword) return false;

            var user = await _userManager.FindByEmailAsync(model.Email);
            if (user == null) return false;

            var result = await _userManager.ResetPasswordAsync(user, model.Token, model.Password);
            return result.Succeeded;
        }


    }
}

