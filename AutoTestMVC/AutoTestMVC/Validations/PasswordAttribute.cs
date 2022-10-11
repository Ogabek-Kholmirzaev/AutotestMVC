using System.ComponentModel.DataAnnotations;

namespace AutoTestMVC.Validations
{
    public class PasswordAttribute:ValidationAttribute
    {
        public int MaxLength { get; set; }

        public PasswordAttribute(int maxLength)
        {
            MaxLength = maxLength;
        }

        public override bool IsValid(object? value)
        {
            var password = (string)value!;

            if(string.IsNullOrEmpty(password))
            {
                ErrorMessage = "Password is null.";
            }
            else if (password.Length < MaxLength)
            {
                ErrorMessage = $"Password length must be minimum length of {MaxLength}.";
            }

            
            return !string.IsNullOrEmpty(password) && password.Length >= MaxLength;
        }
    }
}
