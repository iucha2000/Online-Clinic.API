namespace Online_Clinic.API.DTOs
{
    public class ReservationDto
    {
        public int PatientId { get; set; }
        public int DoctorId { get; set; }
        public string Description { get; set; }
        public DateTime ReservationDate { get; set; }
    }
}
