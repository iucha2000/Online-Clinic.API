using Online_Clinic.API.Models;

namespace Online_Clinic.API.Interfaces
{
    public interface IDoctorRepository : IRepository<Doctor>
    {
        Doctor GetByEmail(string email, bool throwIfNotFound = true);

        Doctor GetByPersonalId(string personalId, bool throwIfNotFound = true);
    }
}
