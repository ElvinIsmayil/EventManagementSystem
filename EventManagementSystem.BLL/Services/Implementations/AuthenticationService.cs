using AutoMapper;
using EventManagementSystem.BLL.Infrastructure;
using EventManagementSystem.BLL.Infrastructure.Interfaces;
using EventManagementSystem.BLL.Services.Interfaces;
using EventManagementSystem.BLL.ViewModels.Auth;
using EventManagementSystem.DAL.Entities;
using EventManagementSystem.DAL.Repositories.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Abstractions;

namespace EventManagementSystem.BLL.Services.Implementations
{
    public class AuthenticationService : IAuthenticationService
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly SignInManager<AppUser> _signInManager;
        private readonly IEmailService _emailService;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IMapper _mapper;
        private readonly IImageService _imageService;
        private readonly IPersonRepository _personRepository;

        public AuthenticationService(UserManager<AppUser> userManager, SignInManager<AppUser> signInManager, IEmailService emailService, RoleManager<IdentityRole> roleManager, IHttpContextAccessor httpContextAccessor, IMapper mapper, IImageService imageService, IPersonRepository personRepository)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _emailService = emailService;
            _roleManager = roleManager;
            _httpContextAccessor = httpContextAccessor;
            _mapper = mapper;
            _imageService = imageService;
            _personRepository = personRepository;
        }

        public async Task<IdentityResult> RegisterAsync(SignUpVM model)
        {
            if (await _userManager.FindByEmailAsync(model.Email) != null)
            {
                return IdentityResult.Failed(new IdentityError { Description = "Email is already registered." });
            }

            var user = _mapper.Map<AppUser>(model);
            user.UserName = model.Email;
            var createResult = await _userManager.CreateAsync(user, model.Password);

            if (!createResult.Succeeded)
            {
                return createResult;
            }

            if (model.IsStudent && model.StudentFile != null)
            {
                user.IsStudentApproved = false;
                (user.StudentImageUrl, List<string> errors) = await _imageService.SaveImageAsync(model.StudentFile, "students", user.Id);
                if (errors.Any())
                {
                    await _userManager.DeleteAsync(user);
                    return IdentityResult.Failed(new IdentityError { Description = "Failed to save student image. Please contact support." });
                }

                await _userManager.UpdateAsync(user);

                if (!await _roleManager.RoleExistsAsync("Student"))
                {
                    var roleCreateResult = await _roleManager.CreateAsync(new IdentityRole("Student"));
                    if (!roleCreateResult.Succeeded)
                    {
                        await _userManager.DeleteAsync(user);
                        return IdentityResult.Failed(new IdentityError { Description = "Failed to create default role. Please contact support." });
                    }
                }

                var roleAssignmentResult = await _userManager.AddToRoleAsync(user, "Student");
                if (!roleAssignmentResult.Succeeded)
                {
                    await _userManager.DeleteAsync(user);
                    return IdentityResult.Failed(new IdentityError { Description = "Failed to assign default role. Please contact support." });
                }
            }


                var person = new Person
                {
                    AppUserId = user.Id,
                };

                var personResult = await _personRepository.AddAsync(person);
                if (personResult == null)
                {
                    await _userManager.DeleteAsync(user);
                    return IdentityResult.Failed(new IdentityError { Description = "Failed to create person record. Please contact support." });
                }

            var emailSent = await SendVerificationEmailAsync(user.Id, model.Email);

            if (!emailSent)
            {
                await _userManager.DeleteAsync(user);
                return IdentityResult.Failed(new IdentityError { Description = "User registered but failed to send confirmation email. Please contact support." });
            }

            return IdentityResult.Success; 
        }

        public async Task<SignInResult> LoginAsync(SignInVM model)
        {
            var user = await _userManager.FindByEmailAsync(model.Email);

            if (user == null)
            {
                return SignInResult.Failed;
            }

            if (!await _userManager.IsEmailConfirmedAsync(user))
            {
                return SignInResult.NotAllowed;
            }
            
            if(_userManager.GetRolesAsync(user).Result.Contains("Student") && !user.IsStudentApproved)
            {
                return SignInResult.NotAllowed; 
            }

            var result = await _signInManager.PasswordSignInAsync(user, model.Password, model.RememberMe, lockoutOnFailure: false);
            return result; 
        }

        public async Task LogoutAsync()
        {
            await _signInManager.SignOutAsync();
        }

        public async Task<bool> IsAuthenticatedAsync(string userId)
        {
            return (_httpContextAccessor.HttpContext?.User?.Identity?.IsAuthenticated ?? false);
        }

        public async Task<string?> GetCurrentUserIdAsync()
        {
            return _userManager.GetUserId(_httpContextAccessor.HttpContext?.User);
        }

        public async Task<string?> GetCurrentUserEmailAsync()
        {
            var user = await _userManager.GetUserAsync(_httpContextAccessor.HttpContext?.User);
            return user?.Email;
        }

        public async Task<IdentityResult> ChangePasswordAsync(string currentPassword, string newPassword, string confirmNewPassword)
        {
            if (newPassword != confirmNewPassword)
            {
                return IdentityResult.Failed(new IdentityError { Description = "New password and confirmation password do not match." });
            }

            var user = await _userManager.GetUserAsync(_httpContextAccessor.HttpContext?.User);
            if (user == null)
            {
                return IdentityResult.Failed(new IdentityError { Description = "User not found. Please log in again." });
            }

            var result = await _userManager.ChangePasswordAsync(user, currentPassword, newPassword);
            return result; 
        }

        public async Task<bool> SendResetPasswordEmailAsync(string email)
        {
            var user = await _userManager.FindByEmailAsync(email);
            if (user == null || !(await _userManager.IsEmailConfirmedAsync(user)))
            {
                return true;
            }

            var token = await _userManager.GeneratePasswordResetTokenAsync(user);
            var resetLink = $"{_httpContextAccessor.HttpContext?.Request.Scheme}://{_httpContextAccessor.HttpContext?.Request.Host}/Auth/ResetPassword?userId={user.Id}&token={Uri.EscapeDataString(token)}";

            var emailBody = EmailTemplateHelper.GetPasswordResetHtml(resetLink);
            await _emailService.SendEmailAsync(email, "Reset Your Password", emailBody);

            return true;
        }

        public async Task<IdentityResult> ConfirmEmailAsync(string userId, string token)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
            {
                return IdentityResult.Failed(new IdentityError { Description = "User not found for email confirmation." });
            }

            var result = await _userManager.ConfirmEmailAsync(user, token);
            return result;
        }

        public async Task<bool> SendVerificationEmailAsync(string userId, string email)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null) return false;

            var token = await _userManager.GenerateEmailConfirmationTokenAsync(user);
            var verificationLink = $"{_httpContextAccessor.HttpContext?.Request.Scheme}://{_httpContextAccessor.HttpContext?.Request.Host}/Auth/ConfirmEmail?userId={userId}&token={Uri.EscapeDataString(token)}";

            var emailBody = EmailTemplateHelper.GetEmailConfirmationHtml(verificationLink);

            return await _emailService.SendEmailAsync(email, "Confirm Your Email", emailBody);
        }

        public async Task<IdentityResult> ResetPasswordAsync(ResetPasswordVM model)
        {
            if (model.Password != model.ConfirmPassword)
            {
                return IdentityResult.Failed(new IdentityError { Description = "Password and confirmation password do not match." });
            }

            var user = await _userManager.FindByEmailAsync(model.Email);
            if (user == null)
            {
                return IdentityResult.Failed(new IdentityError { Description = "Invalid reset request or user not found." });
            }

            var result = await _userManager.ResetPasswordAsync(user, model.Token, model.Password);
            return result; 
        }
    }
}
