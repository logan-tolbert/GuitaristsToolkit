namespace GuitaristsToolkit.Controllers;

using App.Models;
using App.Repo;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

[AutoValidateAntiforgeryToken]
public class SongController : Controller
{
    private readonly ISongRepo _repo;

    public SongController(ISongRepo repo)
    {
        _repo = repo;
    }

    [HttpGet]
    public IActionResult Edit(int id, int setlistId)
    {
        ViewData["ShowLogin"] = false;  

        var song = _repo.GetById(id);

        if (song == null)
        {
            return NotFound();
        }

        ViewBag.SetlistId = setlistId;
        return View(song);
    }


    [HttpPost]
    public IActionResult Edit(Song song, int setlistId)
    {
        if(!ModelState.IsValid)
        {
            ViewBag.SetlistId = setlistId;
            return View(song);
        }

        var currentSong = _repo.GetById(song.Id);

        if(currentSong == null)
        {
            return NotFound();
        }

        _repo.Update(song);

        return RedirectToAction("Edit", "Setlist", new { id = setlistId });
    }

    [HttpPost]
    public IActionResult Delete(int id, int setlistId)
    {
        var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

        if (string.IsNullOrEmpty(userIdClaim))
        {
            return Unauthorized();
        }

        var song = _repo.GetById(id);

        if (song == null)
        {
            return NotFound();
        }

        _repo.Delete(id);

        return RedirectToAction("Edit", "Setlist", new { id = setlistId });
    }



}
