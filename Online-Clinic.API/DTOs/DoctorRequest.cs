using Online_Clinic.API.Enums;
using Online_Clinic.API.Middlewares;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Online_Clinic.API.DTOs
{
    public class DoctorRequest
    {
        [MaxLength(50)]
        public string? FirstName { get; set; }

        [MaxLength(50)]
        public string? LastName { get; set; }

        [MaxLength(50)]
        [EmailAddress]
        public string? Email { get; set; }

        [MaxLength(50)]
        [PasswordPropertyText]
        public string? Password { get; set; }

        [Length(10, 11)]
        public string? Personal_Id { get; set; }

        [ValidEnumValue(typeof(Role), Enums.Role.Admin, Enums.Role.Doctor)]
        public Role? Role { get; set; }

        [ValidEnumValue(typeof(Category))]
        public Category? Category { get; set; }

        [AllowedValues([null, 0, 1, 2, 3, 4, 5])]
        public int? Rating { get; set; }
    }
}
