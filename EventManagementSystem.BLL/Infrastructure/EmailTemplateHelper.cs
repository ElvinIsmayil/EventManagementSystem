// EventManagementSystem.BLL.Infrastructure/EmailTemplateHelper.cs

using System; // For DateTime formatting

namespace EventManagementSystem.BLL.Infrastructure
{
    public class EmailTemplateHelper
    {
        public static string GetEmailConfirmationHtml(string confirmationLink)
        {
            return $@"
<style>html,body {{ padding: 0; margin:0; }}</style>
<div style='font-family:Arial,Helvetica,sans-serif; line-height: 1.5; font-size: 15px; color: #2F3044; background-color:#edf2f7;'>
    <table align='center' width='100%' style='max-width:600px;margin:auto;'>
        <tr>
            <td style='padding: 40px; text-align: center;'>
                <img src='https://www.google.com/url?sa=i&url=https%3A%2F%2Fwww.facebook.com%2Favtolizinqkredit%2F&psig=AOvVaw0cbcgua6KYfH3wKQh68nGJ&ust=1748160906921000&source=images&cd=vfe&opi=89978449&ved=0CBQQjRxqFwoTCJini4DVu40DFQAAAAAdAAAAABAE' alt='Logo' style='height: 45px;' />
            </td>
        </tr>
        <tr>
            <td style='padding: 40px; background: white; border-radius: 6px;'>
                <p style='font-size: 17px;'><strong>Welcome to Credit Management System!</strong></p>
                <p>To activate your account, please click the button below to verify your email address:</p>
                <p style='text-align: center; margin: 30px 0;'>
                    <a href='{confirmationLink}' style='padding: 12px 20px; background-color: #009EF7; color: white; text-decoration: none; border-radius: 4px;'>Activate Account</a>
                </p>
                <p>If the button doesn't work, paste this URL into your browser:</p>
                <p><a href='{confirmationLink}' style='color: #009EF7;'>{confirmationLink}</a></p>
            </td>
        </tr>
    </table>
</div>";
        }

        public static string GetPasswordResetHtml(string resetLink)
        {
            return $@"
<style>html,body {{ padding: 0; margin:0; }}</style>
<div style=""font-family:Arial,Helvetica,sans-serif; line-height: 1.5; font-weight: normal; font-size: 15px; color: #2F3044; min-height: 100%; margin:0; padding:0; width:100%; background-color:#edf2f7"">
    <table align=""center"" border=""0"" cellpadding=""0"" cellspacing=""0"" width=""100%"" style=""border-collapse:collapse;margin:0 auto; padding:0; max-width:600px"">
        <tbody>
            <tr>
                <td align=""center"" valign=""center"" style=""text-align:center; padding: 40px"">
                    <a href=""https://keenthemes.com"" rel=""noopener"" target=""_blank"">
                        <img alt=""Logo"" src=""../../assets/media/logos/logo-1.svg"" class=""h-45px"" />
                    </a>
                </td>
            </tr>
            <tr>
                <td align=""left"" valign=""center"">
                    <div style=""text-align:left; margin: 0 20px; padding: 40px; background-color:#ffffff; border-radius: 6px"">
                        <div style=""padding-bottom: 30px; font-size: 17px;"">
                            <strong>Hello!</strong>
                        </div>
                        <div style=""padding-bottom: 30px"">You are receiving this email because we received a password reset request for your account. To proceed with the password reset please click on the button below:</div>
                        <div style=""padding-bottom: 40px; text-align:center;"">
                            <a href=""{resetLink}"" rel=""noopener"" style=""text-decoration:none;display:inline-block;text-align:center;padding:0.75575rem 1.3rem;font-size:0.925rem;line-height:1.5;border-radius:0.35rem;color:#ffffff;background-color:#009EF7;border:0px;margin-right:0.75rem!important;font-weight:600!important;outline:none!important;vertical-align:middle"" target=""_blank"">Reset Password</a>
                        </div>
                        <div style=""padding-bottom: 30px"">This password reset link will expire in 60 minutes. If you did not request a password reset, no further action is required.</div>
                        <div style=""border-bottom: 1px solid #eeeeee; margin: 15px 0""></div>
                        <div style=""padding-bottom: 50px; word-wrap: break-word;"">
                            <p style=""margin-bottom: 10px;"">Button not working? Try pasting this URL into your browser:</p>
                            <a href=""{resetLink}"" rel=""noopener"" target=""_blank"" style=""text-decoration:none;color: #009EF7"">{resetLink}</a>
                        </div>
                        <div style=""padding-bottom: 10px"">Kind regards,
                        <br>The Keenthemes Team.</div>
                    </div>
                </td>
            </tr>
            <tr>
                <td align=""center"" valign=""center"" style=""font-size: 13px; text-align:center;padding: 20px; color: #6d6e7c;"">
                    <p>Floor 5, 450 Avenue of the Red Field, SF, 10050, USA.</p>
                    <p>Copyright ©
                    <a href=""https://keenthemes.com"" rel=""noopener"" target=""_blank"">Keenthemes</a>.</p>
                </td>
            </tr>
        </tbody>
    </table>
</div>";
        }

        // --- NEW: Event Invitation Email Template ---
        public static string GetInvitationHtml(string inviteeName, string customMessage,
                                               string eventName, DateTime eventStartDate, string eventLocation,
                                               string eventDescription, string invitationLink, string organizerName)
        {
            // Fallback for customMessage if it's empty
            string effectiveCustomMessage = string.IsNullOrWhiteSpace(customMessage)
                ? $"You are invited to the following event:"
                : customMessage;

            // Use the consistent template structure
            return $@"
<style>html,body {{ padding: 0; margin:0; }}</style>
<div style='font-family:Arial,Helvetica,sans-serif; line-height: 1.5; font-size: 15px; color: #2F3044; background-color:#edf2f7;'>
    <table align='center' width='100%' style='max-width:600px;margin:auto;'>
        <tr>
            <td style='padding: 40px; text-align: center;'>
                <img src='https://www.google.com/url?sa=i&url=https%3A%2F%2Fwww.facebook.com%2Favtolizinqkredit%2F&psig=AOvVaw0cbcgua6KYfH3wKQh68nGJ&ust=1748160906921000&source=images&cd=vfe&opi=89978449&ved=0CBQQjRxqFwoTCJini4DVu40DFQAAAAAdAAAAABAE' alt='Event Management Logo' style='height: 45px;' />
            </td>
        </tr>
        <tr>
            <td style='padding: 40px; background: white; border-radius: 6px;'>
                <p style='font-size: 17px;'><strong>Dear {inviteeName},</strong></p>
                <p>{effectiveCustomMessage}</p>
                <h3 style='font-size: 20px; color: #009EF7; margin-bottom: 15px;'>{eventName}</h3>
                <p style='margin-bottom: 5px;'><strong>Date & Time:</strong> {eventStartDate:dddd, MMMM dd,yyyy h:mm tt}</p>
                <p style='margin-bottom: 5px;'><strong>Location:</strong> {eventLocation}</p>
                <p style='margin-bottom: 15px;'><strong>Description:</strong> {eventDescription}</p>

                <p style='text-align: center; margin: 30px 0;'>
                    <a href='{invitationLink}' style='padding: 12px 20px; background-color: #007bff; color: white; text-decoration: none; border-radius: 4px; display: inline-block;'>Respond to Invitation</a>
                </p>

                <p style='margin-top: 20px;'>If the button doesn't work, paste this URL into your browser:</p>
                <p><a href='{invitationLink}' style='color: #007bff;'>{invitationLink}</a></p>

                <div style='border-top: 1px solid #eeeeee; margin-top: 30px; padding-top: 20px;'>
                    <p style='margin-bottom: 5px;'>We look forward to seeing you there!</p>
                    <p>Best regards,<br>The {organizerName} Team</p>
                </div>
            </td>
        </tr>
    </table>
</div>";
        }

        public static string GenerateTicketHtml(
            string eventTitle,
            DateTime eventStartDate, // Changed to DateTime to format within the helper
            string venue,
            string ticketId,
            string attendeeName,
            string orderNumber,
            string seatNumber)
        {
            // Format date and time here to keep the template clean
            string eventDate = eventStartDate.ToString("dd MMM yyyy");
            string eventTime = eventStartDate.ToString("HH:mm");

            return $@"
<!DOCTYPE html>
<html lang=""en"">
<head>
    <meta charset=""UTF-8"" />
    <title>Ticket - {eventTitle}</title>
    <style>
        @import url('https://fonts.googleapis.com/css2?family=Inter:wght@400;600&display=swap');
        body {{ font-family: 'Inter', sans-serif; padding: 60px; background: #eaeaea; }}
        .ticket {{ width: 900px; display: flex; border-radius: 16px; overflow: hidden; background: #fff; box-shadow: 0 12px 30px rgba(0, 0, 0, 0.2); position: relative; }}
        .ticket-left {{ flex: 4; padding: 40px; background: linear-gradient(to bottom right, #0f2027, #203a43, #2c5364); color: #fff; }}
        .ticket-left h1 {{ margin: 0; font-size: 32px; font-weight: 600; }}
        .ticket-left .event-info {{ margin-top: 20px; font-size: 16px; line-height: 1.6; }}
        .ticket-left .info-row {{ display: flex; justify-content: space-between; margin-top: 30px; font-size: 14px; }}
        .ticket-left .info-row div strong {{ display: block; font-size: 16px; margin-bottom: 5px; }}
        .ticket-right {{ flex: 2; padding: 40px; background: #f6f6f6; border-left: 2px dashed #ccc; display: flex; flex-direction: column; justify-content: space-between; }}
        .ticket-right .label {{ font-size: 12px; color: #888; margin-bottom: 5px; }}
        .ticket-right .value {{ font-size: 22px; font-weight: 600; margin-bottom: 20px; color: #333; }}
        .seat-box {{ background: #2c5364; color: white; padding: 15px 25px; border-radius: 10px; font-size: 18px; font-weight: bold; text-align: center; }}
        .ticket:before, .ticket:after {{ content: ''; width: 40px; height: 40px; background: #eaeaea; position: absolute; border-radius: 50%; top: 50%; transform: translateY(-50%); }}
        .ticket:before {{ left: -20px; }} .ticket:after {{ right: -20px; }}
    </style>
</head>
<body>
    <div class=""ticket"">
        <div class=""ticket-left"">
            <h1>{eventTitle}</h1>
            <div class=""event-info"">
                Join us for an unforgettable night of innovation, music, and networking.
            </div>
            <div class=""info-row"">
                <div><strong>Date</strong><span>{eventDate}</span></div>
                <div><strong>Time</strong><span>{eventTime}</span></div>
                <div><strong>Venue</strong><span>{venue}</span></div>
            </div>
        </div>
        <div class=""ticket-right"">
            <div>
                <div class=""label"">Ticket ID</div><div class=""value"">{ticketId}</div>
                <div class=""label"">Name</div><div class=""value"">{attendeeName}</div>
                <div class=""label"">Order #</div><div class=""value"">{orderNumber}</div>
            </div>
            <div class=""seat-box"">Seat: {seatNumber}</div>
        </div>
    </div>
</body>
</html>";
        }
    }
}
    