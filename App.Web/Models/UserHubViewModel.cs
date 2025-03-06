using App.Models;

namespace PracticeTracker.Models
{
    public class UserHubViewModel
    {
        public Guid UserId { get; set; }
        public string Username { get; set; } = string.Empty;
        public List<PracticeSession> PracticeSessions { get; set; } = new List<PracticeSession>();
        public List<SetlistSummary> Setlists { get; set; } = new List<SetlistSummary>();
    }
}
