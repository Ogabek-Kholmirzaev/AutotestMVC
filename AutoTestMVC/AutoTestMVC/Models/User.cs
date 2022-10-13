 using System.ComponentModel.DataAnnotations;
 using AutoTestMVC.Validations;

namespace AutoTestMVC.Models
{
    public class User
    {
        public int Index { get; set; }

        [Required]
        [StringLength(20, MinimumLength = 3)]
        public string? Name { get; set; }

        [Validations.Phone(7)]
        public string? Phone { get; set; }

        [Password(8)]
        public string? Password { get; set; }

        public string? Image { get; set; }
        public IFormFile? ImageFile { get; set; }
    }
}
