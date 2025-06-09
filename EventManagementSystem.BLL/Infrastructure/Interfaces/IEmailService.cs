﻿namespace EventManagementSystem.BLL.Infrastructure.Interfaces
{
    public interface IEmailService
    {
        Task SendEmailAsync(string email, string subject, string message, CancellationToken cancellationToken = default);
    }
}
