namespace App.Models;
using System.ComponentModel.DataAnnotations;

public class SetlistSong
{
    public int SetlistId { get; set; }

    public int SongId { get; set; }

    [Range(1, int.MaxValue, ErrorMessage = "Song order must be at least 1.")]
    public int SongOrder { get; set; }

    [StringLength(1000, ErrorMessage = "Notes cannot be more than 1000 characters.")]
    public string? Notes { get; set; }

    public Song Song { get; set; } = new();
}
