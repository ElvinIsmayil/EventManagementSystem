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
                        <!--begin:Email content-->
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
                        <!--end:Email content-->
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
    }
}