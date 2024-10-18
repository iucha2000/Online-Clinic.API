using System.ComponentModel.DataAnnotations;

namespace Online_Clinic.API.Middlewares
{
    public class ValidEnumValueAttribute : ValidationAttribute
    {
        private readonly Type _enumType;

        public ValidEnumValueAttribute(Type enumType)
        {
            _enumType = enumType;
        }

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            if (value == null || Enum.IsDefined(_enumType, value))
            {
                return ValidationResult.Success;
            }

            return new ValidationResult($"Invalid value for enum {_enumType.Name}.");
        }
    }
}
