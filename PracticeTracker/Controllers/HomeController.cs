namespace GuitaristsToolkit.Controllers;

using App.Models;
using App.Repo;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PracticeTracker.Models;
using System.Diagnostics;



[AutoValidateAntiforgeryToken]
public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;
    private readonly IPracticeSessionRepo _repo;
    private readonly ISetlistRepo _setlistRepo;

    public HomeController(ILogger<HomeController> logger, IPracticeSessionRepo repo, ISetlistRepo setlistRepo)
    {
        _logger = logger;
        _repo = repo;
        _setlistRepo = setlistRepo;

    }

    public IActionResult Index()
    {
        ViewData["ShowLogin"] = true;

        return View();
    }

    public IActionResult Privacy()
    {
        ViewData["ShowLogin"] = false;
        return View();
    }

    [Authorize]
    public IActionResult UserHub()
    {
        ViewData["ShowLogin"] = false;
        
        if (!User.Identity.IsAuthenticated)
        {
            return RedirectToAction("Login", "User");
        }

        try
        {
            var userIdClaim = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;

            if (string.IsNullOrEmpty(userIdClaim))
            {
                return Unauthorized();
            }

            var userId = Guid.Parse(userIdClaim);

            var model = new UserHubViewModel
            {
                UserId = userId,
                Username = User.Identity.Name,
                PracticeSessions = _repo.GetAll()
                    .Where(s => s.UserId == userId)
                    .OrderByDescending(s => s.CreatedAt)
                    .Take(3)
                    .ToList(),
                Setlists = _setlistRepo.GetSetlistsForUser(userId)
                    .Select(setlist => new SetlistSummary
                    {
                        Id = setlist.Id,
                        Title = setlist.Title,
                        CreatedAt = setlist.CreatedAt,
                        SongCount = setlist.SongCount
                    })
                    .OrderByDescending(s => s.CreatedAt)
                    .Take(3)
                    .ToList()
            };

            return View(model);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error loading UserHub for user: {UserId}", User.Identity.Name);
            return RedirectToAction("Error");
        }
    }



    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        ViewData["ShowLogin"] = false;

        var errorId = Activity.Current.Id ?? HttpContext.TraceIdentifier;
        _logger.LogError("An error occurred. Request ID: {RequestId}", errorId);

        return View(new ErrorViewModel { RequestId = errorId });
    }
}
