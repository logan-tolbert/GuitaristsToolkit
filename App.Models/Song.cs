﻿namespace App.Models;
using System.ComponentModel.DataAnnotations;

public class Song
{
    public int Id { get; set; }

    [Required]
    [StringLength(255, MinimumLength = 1)]
    public string Title { get; set; } = string.Empty;

    public string? Key { get; set; }

    [Range(10, 300, ErrorMessage = "BPM must be between 10 and 300.")]
    public int? BPM { get; set; }

    [Range(1, 600, ErrorMessage = "Duration must be between 1 and 600 minutes.")]
    [Display(Name = "Duration (min)")]
    public int? DurationMinutes { get; set; }

    [StringLength(1000, ErrorMessage = "Notes cannot be more than 1000 characters.")]
    public string? Notes { get; set; }
}
