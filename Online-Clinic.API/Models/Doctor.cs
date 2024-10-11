using Online_Clinic.API.Enums;
using System.Text.Json.Serialization;

namespace Online_Clinic.API.Models
{
    public class Doctor : Account
    {
        public Category Category { get; set; }
        public int Rating { get; set; }
        public byte[] Image { get; set; }
        public byte[] CV { get; set; }
        public Reservation[]? Reservations { get; set; }
    }
}
