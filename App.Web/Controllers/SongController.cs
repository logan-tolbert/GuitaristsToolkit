namespace GuitaristsToolkit.Controllers;

using App.Models;
using App.Repo;
using AspNetCoreGeneratedDocument;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using System.Security.Claims;

[AutoValidateAntiforgeryToken]
public class SongController : Controller
{
    private readonly ISongRepo _repo;
    private readonly ISetlistRepo _setlistRepo;

    public SongController(ISongRepo repo, ISetlistRepo setlistRepo)
    {
        _repo = repo;
        _setlistRepo = setlistRepo;
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
        var setlistSong = _setlistRepo.GetSetlistSong(setlistId, id);
        ViewBag.SetlistId = setlistId;
        ViewBag.SongOrder = setlistSong?.SongOrder ?? 1;
        return View(song);
    }


    [HttpPost]
    public IActionResult Edit(Song song, int setlistId, int songOrder)
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
        _setlistRepo.UpdateSetlistSongOrder(setlistId, song.Id, songOrder);
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
