using Online_Clinic.API.Enums;
using Online_Clinic.API.Models;

namespace Online_Clinic.API.DTOs
{
    public class DoctorResponse
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string Personal_Id { get; set; }
        public Role Role { get; set; }
        public Category Category { get; set; }
        public int Rating { get; set; }
    }
}
