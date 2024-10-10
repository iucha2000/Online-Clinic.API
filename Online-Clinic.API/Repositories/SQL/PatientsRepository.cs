using Online_Clinic.API.Interfaces;
using Online_Clinic.API.Models;

namespace Online_Clinic.API.Repositories.SQL
{
    public class PatientsRepository : IRepository<Patient>
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

        public Patient GetEntity(int id)
        {
            throw new NotImplementedException();
        }

        public List<Patient> GetEntities()
        {
            throw new NotImplementedException();
        }
    }
}
