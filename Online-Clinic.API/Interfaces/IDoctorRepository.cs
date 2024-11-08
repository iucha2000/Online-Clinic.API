using Online_Clinic.API.Enums;
using Online_Clinic.API.Models;

namespace Online_Clinic.API.Interfaces
{
    public interface IDoctorRepository : IRepository<Doctor>
    {
        Doctor GetByEmail(string email);

        Doctor GetByPersonalId(string personalId);

        void UploadImage(int doctorId, IFormFile image);

        byte[] GetImage(int doctorId);

        void UploadCV(int doctorId, IFormFile cv);

        byte[] GetCV(int doctorId);

        List<CategoryInfo> GetCategoryList();
    }
}
