using EventManagementSystem.BLL.ViewModels.Auth;

namespace EventManagementSystem.BLL.Services.Interfaces
{
    public interface IAuthenticationService
    {
        Task<bool> RegisterAsync(SignUpVM model);
        Task<bool> SendVerificationEmailAsync(string userId, string email);
        Task<bool> ConfirmEmailAsync(string userId, string token);

        Task<bool> LoginAsync(SignInVM model);
        Task LogoutAsync();
        Task<bool> IsAuthenticatedAsync(string userId);

        Task<string?> GetCurrentUserIdAsync();
        Task<string?> GetCurrentUserEmailAsync();

        Task<bool> ChangePasswordAsync(string currentPassword, string newPassword, string confirmNewPassword);
        Task<bool> SendResetPasswordEmailAsync(string email);
        Task<bool> ResetPasswordAsync(ResetPasswordVM model);

    }
}
