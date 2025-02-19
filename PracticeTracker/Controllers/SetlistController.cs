namespace GuitaristsToolkit.Controllers;

using App.Models;
using App.Repo;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;
using System.Security.Claims;


[AutoValidateAntiforgeryToken]
public class SetlistController : Controller
{
    private readonly ISetlistRepo _repo;
    private readonly ISongRepo _songRepo;

    public SetlistController(ISetlistRepo repo, ISongRepo songRepo)
    {
        _repo = repo;
        _songRepo = songRepo;
    }

    [HttpGet]
    public IActionResult Create()
    {
        ViewData["ShowLogin"] = false;
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
            SongOrder = 0,
            Notes = song.Notes
        };

        _repo.AddSongToSetlist(setlistSong);

        return RedirectToAction("Edit", new { id = setlistId });
    }

    [HttpGet]
    public IActionResult ViewAll()
    {
        ViewData["ShowLogin"] = false;
        var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

        if (string.IsNullOrEmpty(userIdClaim))
        {
            return Unauthorized();
        }

        var userId = Guid.Parse(userIdClaim);
        var setlists = _repo.GetSetlistsForUser(userId);

        return View(setlists);
    }


    [HttpGet]
    public IActionResult Details(int id)
    {
        ViewData["ShowLogin"] = false;
        var setlist = _repo.GetSetlistWithSongs(id);
        return View(setlist);
    }

    [HttpGet]
    public IActionResult Edit(int id)
    {
        ViewData["ShowLogin"] = false;
        var setlist = _repo.GetSetlistWithSongs(id);
        var songs = _repo.GetAll();

        ViewBag.Songs = songs;
        return View(setlist);
    }

    [HttpPost]
    public IActionResult Delete(int id)
    {
        var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

        if (string.IsNullOrEmpty(userIdClaim))
        {
            return Unauthorized();
        }

        try
        {
            _repo.Delete(id);
            
            return RedirectToAction("ViewAll", "Setlist");
        }
        catch (Exception ex)
        {
            return RedirectToAction("ViewAll", "Setlist");
        }
    }


}
