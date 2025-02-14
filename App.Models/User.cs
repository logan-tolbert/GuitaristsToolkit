using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App.Models
{
    public class User
    {
        public Guid Id { get; set; } = Guid.NewGuid();

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
        public string PasswordHash { get; set; } = string.Empty;
       
        public DateTime CreatedAt { get; set; }
    }
}
