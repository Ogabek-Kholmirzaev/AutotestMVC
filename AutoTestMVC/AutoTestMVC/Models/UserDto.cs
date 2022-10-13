using AutoTestMVC.Validations;

namespace AutoTestMVC.Models
{
    public class UserDto
    {
        [Validations.Phone(7)]
        public string? Phone { get; set; }

        [Password(8)]
        public string? Password { get; set; }
    }
}
