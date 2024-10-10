using Online_Clinic.API.Enums;

namespace Online_Clinic.API.Models
{
    public abstract class Account
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string Personal_Id { get; set; }
        public int ActivationCode { get; set; }
        public Role Role { get; set; }
    }
}
