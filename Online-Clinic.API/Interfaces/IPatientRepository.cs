using Online_Clinic.API.Models;

namespace Online_Clinic.API.Interfaces
{
    public interface IPatientRepository : IRepository<Patient>
    {
        Patient GetByEmail(string email, bool throwIfNotFound = true);

        Patient GetByPersonalId (string personalId, bool throwIfNotFound = true);
    }
}
