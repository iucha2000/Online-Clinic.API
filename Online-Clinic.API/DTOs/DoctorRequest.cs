using Online_Clinic.API.Enums;

namespace Online_Clinic.API.DTOs
{
    public class DoctorRequest
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string Personal_Id { get; set; }
        public int ActivationCode { get; set; }
        public Role Role { get; set; }
        public Category Category { get; set; }
        public int Rating { get; set; }
    }
}
