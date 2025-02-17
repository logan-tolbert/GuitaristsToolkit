using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace App.Models
{
    public class SetlistSummary 
    {
        public int Id { get; set; }

        [Required]
        [StringLength(255, MinimumLength = 1, ErrorMessage = "Title must be between 1 and 255 characters.")]
        public string Title { get; set; } = string.Empty;

        public DateTime CreatedAt { get; set; }

        public int SongCount { get; set; } = 0;
    }
}