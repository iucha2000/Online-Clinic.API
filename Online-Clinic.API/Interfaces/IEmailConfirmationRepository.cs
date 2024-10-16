using Online_Clinic.API.Models;

namespace Online_Clinic.API.Interfaces
{
    public interface IEmailConfirmationRepository
    {
        void AddToDatabase(EmailConfirmation emailConfirmation);

        EmailConfirmation GetFromDatabaseByEmail(string email);
    }
}
