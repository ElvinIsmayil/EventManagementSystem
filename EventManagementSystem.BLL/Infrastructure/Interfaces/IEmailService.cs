namespace EventManagementSystem.BLL.Infrastructure.Interfaces
{
    public interface IEmailService
    {
        Task<bool> SendEmailAsync(string email, string subject, string message, CancellationToken cancellationToken = default);
    }
}
