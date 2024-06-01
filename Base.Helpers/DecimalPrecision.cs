using System.ComponentModel.DataAnnotations;

namespace Base.Helpers
{
    public class DecimalPrecisionAttribute : ValidationAttribute
    {
        private readonly int _precision;

        public DecimalPrecisionAttribute(int precision)
        {
            _precision = precision;
        }

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            decimal number = (decimal)value;

            if (Decimal.Round(number, _precision) != number)
            {
                return new ValidationResult($"Number cannot have more than {_precision} decimal places.");
            }

            return ValidationResult.Success;
        }
    }
}
