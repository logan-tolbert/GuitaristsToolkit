namespace App.Models;
using System.ComponentModel.DataAnnotations;

public class Setlist
{
    public int Id { get; set; }

    [Required]
    public Guid UserId { get; set; }

    [Required]
    [StringLength(255, MinimumLength = 1, ErrorMessage = "Setlist name must be between 1 and 255 characters.")]
    public string Name { get; set; } = string.Empty;

    [Required]
    public DateTime CreatedAt { get; set; }

    public List<SetlistSong> SetlistSongs { get; set; } = new();

    public Song NewSong { get; set; } = new();
}
