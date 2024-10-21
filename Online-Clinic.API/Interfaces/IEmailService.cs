namespace Online_Clinic.API.Interfaces
{
    public interface IEmailService
    {
        Task SendVerificationEmailAsync(string email, string subject, int code);

        int GenerateConfirmationCode();

        Task SendConfirmationCodeAsync(string email);

        bool VerifyCode(string email, int code);

        Task SendPasswordResetEmailAsync(string email, string subject, string newPassword);

        string GenerateNewPassword();

        Task SendNewPasswordAsync(string email);
    }
}
