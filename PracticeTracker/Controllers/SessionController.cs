using App.Models;
using App.Repo;
using Microsoft.AspNetCore.Mvc;

namespace PracticeTracker.Controllers
{
    public class SessionController : Controller
    {
        private readonly IPracticeSessionRepo _session;
        public SessionController(IPracticeSessionRepo session)
        {
            _session = session;
        }

        //GET: Session/Create
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(PracticeSession session)
        {
             session.UserId = 1;
            if (ModelState.IsValid)
            {
                _session.Create(session);
                return RedirectToAction("Sessions", "Home");
            }
            return View(session);
           
        }
    }
}
