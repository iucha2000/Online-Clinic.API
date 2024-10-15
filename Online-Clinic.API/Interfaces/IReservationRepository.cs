using Online_Clinic.API.Models;

namespace Online_Clinic.API.Interfaces
{
    public interface IReservationRepository : IRepository<Reservation>
    {
        List<Reservation> GetByPatientId(int patientId);

        List<Reservation> GetByDoctorId(int doctorId);
    }
}
