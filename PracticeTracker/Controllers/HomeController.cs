using System.Diagnostics;
using App.Repo;
using AspNetCoreGeneratedDocument;
using Microsoft.AspNetCore.Mvc;
using PracticeTracker.Models;

namespace PracticeTracker.Controllers;

public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;
    private readonly IPracticeSessionRepo _repo;

    public HomeController(ILogger<HomeController> logger, IPracticeSessionRepo repo)
    {
        _logger = logger;
        _repo = repo;

    }

    public IActionResult Index()
    {
        return View();
    }

    public IActionResult Privacy()
    {
        return View();
    }

    public IActionResult UserHub()
    {
        var sessions = _repo.GetAll();
        return View(sessions);
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
