using System.ComponentModel.DataAnnotations;

namespace GuitaristsToolkit.Models
{
    public class LoginRequestViewModel
    {
        [Required]
        [Display(Name = "Email or Username")]
        public string EmailOrUsername { get; set; } = string.Empty;

        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; } = string.Empty;
    }
}
