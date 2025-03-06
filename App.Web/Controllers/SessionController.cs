namespace PracticeTracker.Controllers;

using App.Models;
using App.Repo;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

[Authorize]
[AutoValidateAntiforgeryToken]
public class SessionController : Controller
{
    private readonly IPracticeSessionRepo _repo;
    public SessionController(IPracticeSessionRepo repo)
    {
        _repo = repo;
    }

    public IActionResult ViewAll()
    {
        ViewData["ShowLogin"] = false;
        var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        if (string.IsNullOrEmpty(userIdClaim))
        {
            return Unauthorized();
        }

        var userId = Guid.Parse(userIdClaim);
        var sessions = _repo.GetAll().Where(s => s.UserId == userId).ToList();

        return View(sessions);
    }


    public IActionResult Create()
    {
        ViewData["ShowLogin"] = false;
        var session = new PracticeSession
        {
            CreatedAt = DateTime.UtcNow 
        };
        return View(session);
    }

    [HttpPost]
    public IActionResult Create(PracticeSession session)
    {
        var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        if (string.IsNullOrEmpty(userIdClaim))
        {
            return Unauthorized();
        }

        session.UserId = Guid.Parse(userIdClaim);
        session.CreatedAt = DateTime.UtcNow;

        ModelState.Remove("User");

        if (!ModelState.IsValid)
        {
            return View(session);
        }

        try
        {
            _repo.Create(session);
            return RedirectToAction("UserHub", "Home");
        }
        catch (Exception)
        {
            ModelState.AddModelError(string.Empty, "An error occurred while saving the session.");
            return View(session);
        }
    }

    [HttpGet]
    public IActionResult Edit(int id)
    {
        ViewData["ShowLogin"] = false;
        var session = _repo.GetById(id);
        if (session == null)
        {
            return NotFound();
        }
        return View(session);
    }

    [HttpPost]
    public IActionResult Edit(PracticeSession session)
    {
        var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        if (string.IsNullOrEmpty(userIdClaim))
        {
            return Unauthorized();
        }

        var userId = Guid.Parse(userIdClaim);
        var existingSession = _repo.GetById(session.Id);

        if (existingSession == null || existingSession.UserId != userId)
        {
            return Forbid(); 
        }

        session.UserId = userId; 
        if (!ModelState.IsValid)
        {
            return View(session);
        }

        _repo.Update(session);
        return RedirectToAction("UserHub", "Home");
    }

    public IActionResult Delete(int id)
    {
        var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

        if (string.IsNullOrEmpty(userIdClaim))
        {
            return Unauthorized();
        }

        _repo.Delete(id);
        return RedirectToAction("UserHub", "Home");
    }
}
