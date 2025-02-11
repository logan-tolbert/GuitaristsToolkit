namespace App.Models
{
    public class SetlistSongResult
        {
            public int Id { get; set; }
            public int UserId { get; set; }
            public string Name { get; set; }
            public DateTime CreatedAt { get; set; }
            public int? SongId { get; set; }
            public int? SongOrder { get; set; }
            public string Notes { get; set; } 
            public string Title { get; set; } 
            public string Key { get; set; }
            public int? BPM { get; set; } 
            public int? DurationMinutes { get; set; }
        }
    
}
