namespace Online_Clinic.API.Interfaces
{
    public interface IEmailService
    {
        Task SendVerificationEmailAsync(string email, string subject, int code);

        int GenerateConfirmationCode();

        Task SendConfirmationCodeAsync(string email);

        bool VerifyCode(string email, int code);
    }
}
