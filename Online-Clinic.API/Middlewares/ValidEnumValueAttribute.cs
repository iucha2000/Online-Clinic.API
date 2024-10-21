using System.ComponentModel.DataAnnotations;

namespace Online_Clinic.API.Middlewares
{
    public class ValidEnumValueAttribute : ValidationAttribute
    {
        private readonly Type _enumType;
        private readonly object[] _allowedValues;

        public ValidEnumValueAttribute(Type enumType, params object[] allowedValues)
        {
            _enumType = enumType;
            _allowedValues = allowedValues;
        }

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            if (value == null)
            {
                return ValidationResult.Success;
            }

            if (!Enum.IsDefined(_enumType, value))
            {
                return new ValidationResult($"Invalid value for enum {_enumType.Name}.");
            }

            if (_allowedValues.Length > 0 && !_allowedValues.Contains(value))
            {
                return new ValidationResult($"The value '{value}' is not allowed for this field.");
            }

            return ValidationResult.Success;
        }
    }
}
