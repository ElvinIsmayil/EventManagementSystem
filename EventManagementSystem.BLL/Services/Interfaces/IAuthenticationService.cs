namespace EventManagementSystem.BLL.Services.Interfaces
{
    public interface IAuthenticationService
    {
        Task<bool> RegisterAsync(string email, string password, string confirmPassword);
        Task<bool> SendVerificationEmailAsync(string userId, string email);
        Task<bool> ConfirmEmailAsync(string userId, string token);

        Task<bool> LoginAsync(string email, string password, bool rememberMe);
        Task LogoutAsync();
        Task<bool> IsAuthenticatedAsync(string userId);

        Task<string?> GetCurrentUserIdAsync();
        Task<string?> GetCurrentUserEmailAsync();

        Task<bool> ChangePasswordAsync(string currentPassword, string newPassword, string confirmNewPassword);
        Task<bool> SendResetPasswordEmailAsync(string email);
        Task<bool> ResetPasswordAsync(string userId, string token, string newPassword, string confirmNewPassword);

    }
}
