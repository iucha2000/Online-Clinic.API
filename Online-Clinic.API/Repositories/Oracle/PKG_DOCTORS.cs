using Online_Clinic.API.Interfaces;
using Online_Clinic.API.Models;

namespace Online_Clinic.API.Repositories.Oracle
{
    public class PKG_DOCTORS : PKG_BASE, IDoctorRepository
    {
        public PKG_DOCTORS(IConfiguration configuration) : base(configuration)
        {
        }

        public void AddEntity(Doctor entity)
        {
            throw new NotImplementedException();
        }

        public void UpdateEntity(int id, Doctor entity)
        {
            throw new NotImplementedException();
        }

        public void DeleteEntity(int id)
        {
            throw new NotImplementedException();
        }

        public Doctor GetEntity(int id, bool throwIfNotFound = true)
        {
            throw new NotImplementedException();
        }

        public List<Doctor> GetEntities()
        {
            throw new NotImplementedException();
        }
    }
}
