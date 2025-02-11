using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App.Models
{
    
    public class SetlistSong
    {
        public int SetlistId { get; set; }
        public int SongId { get; set; }
        public int SongOrder { get; set; }
        public string Notes { get; set; } = string.Empty;

        public Song Song { get; set; } = new();
    }
}
