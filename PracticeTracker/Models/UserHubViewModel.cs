using App.Models;

namespace PracticeTracker.Models
{
    public class UserHubViewModel
    {
        public int UserId { get; set; }
        public string Username { get; set; } = null!;
        public List<SetlistSummary> Setlists { get; set; } = new List<SetlistSummary>();
    }
}
