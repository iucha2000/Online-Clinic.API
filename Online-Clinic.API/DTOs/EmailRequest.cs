using System.ComponentModel.DataAnnotations;

namespace Online_Clinic.API.DTOs
{
    public class EmailRequest
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        public string Subject { get; set; }

        [Required]
        public string Message { get; set; }
    }
}
