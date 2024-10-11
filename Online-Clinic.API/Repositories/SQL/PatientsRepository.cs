using Online_Clinic.API.Interfaces;
using Online_Clinic.API.Models;

namespace Online_Clinic.API.Repositories.SQL
{
    public class PatientsRepository : IPatientRepository
    {
        public void AddEntity(Patient entity)
        {
            throw new NotImplementedException();
        }
        public void UpdateEntity(int id, Patient entity)
        {
            throw new NotImplementedException();
        }

        public void DeleteEntity(int id)
        {
            throw new NotImplementedException();
        }

        public Patient GetEntity(int id, bool throwIfNotFound = true)
        {
            throw new NotImplementedException();
        }

        public List<Patient> GetEntities()
        {
            throw new NotImplementedException();
        }

        public Patient GetByEmail(string email, bool throwIfNotFound = true)
        {
            throw new NotImplementedException();
        }

        public Patient GetByPersonalId(string personalId, bool throwIfNotFound = true)
        {
            throw new NotImplementedException();
        }
    }
}
