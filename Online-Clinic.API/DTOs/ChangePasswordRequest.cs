using System.ComponentModel.DataAnnotations;

namespace Online_Clinic.API.DTOs
{
    public class ChangePasswordRequest
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        public string Password { get; set; }
    }
}
