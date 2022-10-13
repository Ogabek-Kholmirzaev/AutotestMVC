using System.ComponentModel.DataAnnotations;

namespace AutoTestMVC.Validations
{
    public class PhoneAttribute:ValidationAttribute
    {
        public int MaxLength = 7;

        public PhoneAttribute(int maxLength)
        {
            MaxLength = maxLength;
        }

        public override bool IsValid(object? value)
        {
            var _value = (string?)value;

            if (string.IsNullOrEmpty(_value))
            {
                ErrorMessage = "Phone is null";
                return false;
            }

            if (_value.Length < MaxLength)
            {
                ErrorMessage = $"Phone must be minimum length of {MaxLength}";
                return false;
            }

            var isNUmber = long.TryParse(_value, out var number);
            if (!isNUmber)
            {
                ErrorMessage = "Phone is not valid";
                return false;
            }

            return true;
        }
    }
}
