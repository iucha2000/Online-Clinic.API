namespace Online_Clinic.API.Models
{
    public class Reservation
    {
        public int Id { get; set; }
        public int PatientId { get; set; }
        public int DoctorId { get; set; }
        public string? Description { get; set; }
        public DateTime? ReservationDate { get; set; }
    }
}
