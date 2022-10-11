using System.ComponentModel.DataAnnotations;

namespace AutoTestMVC.Validations
{
    public class PhoneAttribute:ValidationAttribute
    {
        public override bool IsValid(object? value)
        {
            ErrorMessage = "Phone is null.";
            return !string.IsNullOrEmpty((string)value!);
        }
    }
}
