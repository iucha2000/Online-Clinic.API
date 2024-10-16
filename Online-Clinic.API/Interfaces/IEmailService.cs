namespace Online_Clinic.API.Interfaces
{
    public interface IEmailService
    {
        Task SendEmailAsync(string email, string subject, string message);

        int GenerateConfirmationCode();

        Task SendConfirmationCodeAsync(string email);

        bool VerifyCode(string email, int code);
    }
}
