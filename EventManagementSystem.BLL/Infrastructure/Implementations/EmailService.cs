using EventManagementSystem.BLL.Configurations;
using EventManagementSystem.BLL.Infrastructure.Interfaces;
using Microsoft.Extensions.Options;
using System.Net;
using System.Net.Mail;

namespace EventManagementSystem.BLL.Infrastructure.Implementations
{
    public sealed class EmailService : IEmailService
    {
        private readonly SmtpSettings _smtpSettings;

        public EmailService(IOptions<SmtpSettings> smtpOptions)
        {
            _smtpSettings = smtpOptions.Value ?? throw new ArgumentNullException(nameof(smtpOptions));
        }

        public async Task SendEmailAsync(string email, string subject, string htmlMessage, CancellationToken cancellationToken = default)
        {
            using var smtp = new SmtpClient(_smtpSettings.Host)
            {
                Port = _smtpSettings.Port,
                Credentials = new NetworkCredential(_smtpSettings.Username, _smtpSettings.Password),
                EnableSsl = _smtpSettings.EnableSsl
            };

            using var mail = new MailMessage(_smtpSettings.Username, email, subject, htmlMessage)
            {
                IsBodyHtml = true
            };

            try
            {
                await smtp.SendMailAsync(mail, cancellationToken);
            }
            catch (SmtpException ex)
            {
                throw new InvalidOperationException("Failed to send email.", ex);
            }
        }
    }
}
