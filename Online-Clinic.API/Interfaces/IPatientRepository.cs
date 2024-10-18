using Online_Clinic.API.Models;

namespace Online_Clinic.API.Interfaces
{
    public interface IPatientRepository : IRepository<Patient>
    {
        Patient GetByEmail(string email);

        Patient GetByPersonalId (string personalId);
    }
}
