using Online_Clinic.API.Models;

namespace Online_Clinic.API.Interfaces
{
    public interface IAccountRepository
    {
        Account ValidateUser(string email, string password);

        void ValidatePassword(string storedPassword, string providedPassword);

        bool EmailExists(string email);

        bool PersonalIdExists(string personalId);

        Patient GetPatientByEmail(string email, bool throwIfNotFound = true);

        Doctor GetDoctorByEmail(string email, bool throwIfNotFound = true);

        Patient GetPatientByPersonalId(string personalId, bool throwIfNotFound = true);

        Doctor GetDoctorByPersonalId(string personalId, bool throwIfNotFound = true);
    }
}
