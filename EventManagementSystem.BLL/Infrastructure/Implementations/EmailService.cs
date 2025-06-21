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

        public async Task<bool> SendEmailAsync(string email, string subject, string htmlMessage, CancellationToken cancellationToken = default)
        {
            try
            {
                using var smtp = new SmtpClient(_smtpSettings.Host)
                {
                    Port = _smtpSettings.Port,
                    Credentials = new NetworkCredential(_smtpSettings.Username, _smtpSettings.Password),
                    EnableSsl = _smtpSettings.EnableSsl,
                    DeliveryMethod = SmtpDeliveryMethod.Network,
                    Timeout = 10000,
                    UseDefaultCredentials = false
                };

                using var mail = new MailMessage();
                mail.From = new MailAddress(_smtpSettings.Username, _smtpSettings.Username);
                mail.To.Add(email);
                mail.Subject = subject;
                mail.Body = htmlMessage;
                mail.IsBodyHtml = true;

                await smtp.SendMailAsync(mail, cancellationToken);
                return true;
            }
            catch (SmtpException)
            {
                return false;
            }
            catch (Exception)
            {
                return false;
            }
        }
    }
}
