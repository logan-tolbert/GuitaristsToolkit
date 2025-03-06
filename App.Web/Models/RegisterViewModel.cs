using System.ComponentModel.DataAnnotations;

namespace GuitaristsToolkit.Models
{
    public class RegisterViewModel
    {
        [Required]
        [MinLength(3)]
        [MaxLength(30)]
        public string Username { get; set; } = string.Empty;

        [Required]
        [MinLength(1)]
        [MaxLength(75)]
        public string FirstName { get; set; } = string.Empty;

        [Required]
        [MinLength(1)]
        [MaxLength(75)]
        public string LastName { get; set; } = string.Empty;

        [Required]
        [EmailAddress]
        [MaxLength(255)]
        public string Email { get; set; } = string.Empty;

        [Required]
        [MinLength(8)]
        [MaxLength(100)]
        [DataType(DataType.Password)]
        public string Password { get; set; } = string.Empty;

        [Required]
        [Compare("Password", ErrorMessage = "Passwords do not match.")]
        [DataType(DataType.Password)]
        public string ConfirmPassword { get; set; } = string.Empty;

    }
}
