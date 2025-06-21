using EventManagementSystem.BLL.ViewModels.Auth;
using Microsoft.AspNetCore.Identity;

namespace EventManagementSystem.BLL.Services.Interfaces
{
    public interface IAuthenticationService
    {
        Task<IdentityResult> RegisterAsync(SignUpVM model);
        Task<bool> SendVerificationEmailAsync(string userId, string email);
        Task<IdentityResult> ConfirmEmailAsync(string userId, string token);

        Task<SignInResult> LoginAsync(SignInVM model);
        Task LogoutAsync();
        Task<bool> IsAuthenticatedAsync(string userId);

        Task<string?> GetCurrentUserIdAsync();
        Task<string?> GetCurrentUserEmailAsync();

        Task<IdentityResult> ChangePasswordAsync(string currentPassword, string newPassword, string confirmNewPassword);
        Task<bool> SendResetPasswordEmailAsync(string email);
        Task<IdentityResult> ResetPasswordAsync(ResetPasswordVM model);
    }
}
