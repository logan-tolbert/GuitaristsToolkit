using App.Data.Context;
using App.Models;
using App.Repo;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;
using System.Security.Claims;

namespace GuitaristsToolkit.Controllers
{
    public class SetlistController : Controller
    {
        private readonly ISetlistRepo _repo;
        private readonly ISongRepo _songRepo;

        public SetlistController(ISetlistRepo repo, ISongRepo songRepo)
        {
            _repo = repo;
            _songRepo = songRepo;
        }

        public IActionResult Index(int id)
        {
            var setlist = _repo.GetSetlistWithSongs(id);
            return View(setlist);
        }

        [HttpGet]
        public IActionResult Create()
        {
            ViewBag.Songs = _repo.GetAll();
            return View();
        }

        [HttpPost]
        public IActionResult Create(Setlist setlist)
        {
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (string.IsNullOrEmpty(userIdClaim))
            {
                return Unauthorized(); 
            }

            setlist.UserId = Guid.Parse(userIdClaim); 
            setlist.CreatedAt = DateTime.UtcNow;

            int setListId = _repo.Create(setlist);

            return RedirectToAction("Edit", new { id = setListId });
        }

        [HttpPost]
        public IActionResult AddSong(int setlistId, string songTitle, string? songKey, int? songBPM, int? songDuration, string? notes)
        {
            var song = new Song
            {
                Title = songTitle,
                Key = songKey,
                BPM = songBPM,
                DurationMinutes = songDuration,
                Notes = notes
            };

            int songId = _songRepo.Create(song);

            var setlistSong = new SetlistSong
            {
                SetlistId = setlistId,
                SongId = songId,
                SongOrder = 0
            };

            _repo.AddSongToSetlist(setlistSong);

            return RedirectToAction("Edit", new { id = setlistId });
        }

        [HttpGet]
        public IActionResult GetUserSetlists()
        {
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (string.IsNullOrEmpty(userIdClaim))
            {
                return Unauthorized();
            }

            var userId = Guid.Parse(userIdClaim);
            var setlists = _repo.GetSetlistsForUser(userId);

            Console.WriteLine("Setlist Results: " + JsonSerializer.Serialize(setlists));
            return Json(setlists);
        }

        [HttpGet]
        public IActionResult Details(int id)
        {
            var setlist = _repo.GetSetlistWithSongs(id);
            return View(setlist);
        }

        [HttpGet]
        public IActionResult Edit(int id)
        {
            var setlist = _repo.GetSetlistWithSongs(id);
            var songs = _repo.GetAll();

            ViewBag.Songs = songs;
            return View(setlist);
        }
    }
}
