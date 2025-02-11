using App.Data.Context;
using App.Repo;
using Microsoft.AspNetCore.Mvc;

namespace PracticeTracker.Controllers
{
    public class SetlistController : Controller
    {
        private ISetlistRepo _repo;

        public SetlistController(ISetlistRepo repo)
        {
            _repo = repo;
        }
        public IActionResult Index(int id)
        {
            var setlist = _repo.GetSetlistWithSongs(id);
            return View(setlist);
        }
    }
}
