using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App.Models
{
    public class Song
    {
        public int Id { get; set; }
        public string Title { get; set; } = null!;
        public string? Key { get; set; }
        public int? BPM { get; set; }
        public int? DurationMinutes { get; set; }
        public string? Notes { get; set; }
    }
}
