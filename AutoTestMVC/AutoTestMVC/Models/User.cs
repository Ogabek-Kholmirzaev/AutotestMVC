 using System.ComponentModel.DataAnnotations;
 using AutoTestMVC.Validations;

namespace AutoTestMVC.Models
{
    public class User
    {
        public int Index { get; set; }
        public string? Name { get; set; }

        [Validations.Phone]
        [Required]
        [StringLength(9, MinimumLength = 7)]
        public string? Phone { get; set; }

        [Password(8)]
        [Required]
        [StringLength(20, MinimumLength = 6)]
        public string? Password { get; set; }
    }
}
