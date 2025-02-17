namespace App.Models;
using System.ComponentModel.DataAnnotations;

public class PracticeSession
{
    public int Id { get; set; }

    [Required]
    public Guid UserId { get; set; }

    [Required]
    public DateTime CreatedAt { get; set; }

    [Required]
    [Range(1, 600, ErrorMessage = "Duration must be between 1 and 600 minutes.")]
    [Display(Name = "Duration (min)")]
    public int DurationMinutes { get; set; }

    [Required]
    [StringLength(100, MinimumLength = 3, ErrorMessage = "Focus area must be between 3 and 100 characters.")]
    [Display(Name = "Focus Area")]
    public string FocusArea { get; set; } = string.Empty!;

    [Display(Name = "Session Notes")]
    [StringLength(1000, ErrorMessage = "Notes cannot be more than 1000 characters.")]
    public string? Notes { get; set; }

    public virtual User? User { get; set; }
}
