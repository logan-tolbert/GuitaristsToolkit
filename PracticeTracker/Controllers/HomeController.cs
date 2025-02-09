using System.Diagnostics;
using App.Repo;
using AspNetCoreGeneratedDocument;
using Microsoft.AspNetCore.Mvc;
using PracticeTracker.Models;

namespace PracticeTracker.Controllers;

public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;
    private readonly IPracticeSessionRepo _session;

    public HomeController(ILogger<HomeController> logger, IPracticeSessionRepo session)
    {
        _logger = logger;
        _session = session;

    }

    public IActionResult Index()
    {
        return View();
    }

    public IActionResult Privacy()
    {
        return View();
    }

    public IActionResult Sessions()
    {
        var sessions = _session.GetAll();
        return View(sessions);
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
