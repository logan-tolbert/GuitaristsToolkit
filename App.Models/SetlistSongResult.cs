namespace App.Models;
using System.ComponentModel.DataAnnotations;

public class SetlistSongResult
{
    public int Id { get; set; }

    [Required]
    public Guid UserId { get; set; }

    [Required]
    [StringLength(255, MinimumLength = 1, ErrorMessage = "Setlist name must be between 1 and 255 characters.")]
    public string Name { get; set; } = string.Empty;

    [Required]
    public DateTime CreatedAt { get; set; }

    public int? SongId { get; set; }

    [Range(1, int.MaxValue, ErrorMessage = "Song order must be at least 1.")]
    public int? SongOrder { get; set; }

    [StringLength(1000, ErrorMessage = "Notes cannot be more than 1000 characters.")]
    public string? Notes { get; set; }

    [Required]
    [StringLength(255, MinimumLength = 1, ErrorMessage = "Song title must be between 1 and 255 characters.")]
    public string Title { get; set; } = string.Empty;

    [RegularExpression(@"^[A-Ga-g][#b]?[m]?$", ErrorMessage = "Invalid musical key format.")]
    public string? Key { get; set; }

    [Range(30, 300, ErrorMessage = "BPM must be between 30 and 300.")]
    public int? BPM { get; set; }

    [Range(1, 600, ErrorMessage = "Duration must be between 1 and 600 minutes.")]
    [Display(Name = "Duration (min)")]
    public int? DurationMinutes { get; set; }
}

