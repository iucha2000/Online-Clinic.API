using System.Text.Json.Serialization;

namespace Online_Clinic.API.Models
{
    public class Login
    {
        public string Email { get; set; }
        public string Password { get; set; }
    }
}
