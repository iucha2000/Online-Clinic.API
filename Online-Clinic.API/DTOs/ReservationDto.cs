using System.ComponentModel.DataAnnotations;

namespace Online_Clinic.API.DTOs
{
    public class ReservationDto
    {
        [Required]
        public int? PatientId { get; set; }

        [Required]
        public int? DoctorId { get; set; }

        [MaxLength(100)]
        public string? Description { get; set; }

        public DateTime? ReservationDate { get; set; }
    }
}
