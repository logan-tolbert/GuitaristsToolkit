using System.Diagnostics;
using App.Repo;
using Microsoft.AspNetCore.Mvc;
using PracticeTracker.Models;

namespace GuitaristsToolkit.Controllers;

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

    public IActionResult UserHub()
    {
        ViewData["ShowLogin"] = false;
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
            PracticeSessions = _repo.GetAll().Where(s => s.UserId == userId).ToList(),
            Setlists = _setlistRepo.GetSetlistsForUser(userId).ToList()
        };

        return View(model);
    }


    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        ViewData["ShowLogin"] = false;
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
