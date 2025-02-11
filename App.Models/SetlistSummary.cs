﻿using System.Text.Json.Serialization;

namespace App.Models
{
    public class SetlistSummary 
    {
        public int Id { get; set; }
        public string Title { get; set; } = null!;
        public DateTime CreatedAt { get; set; }
        public int SongCount { get; set; }
    }
}