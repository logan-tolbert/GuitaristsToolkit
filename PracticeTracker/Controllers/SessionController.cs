using App.Models;
using App.Repo;
using Microsoft.AspNetCore.Mvc;

namespace PracticeTracker.Controllers
{
    public class SessionController : Controller
    {
        private readonly IPracticeSessionRepo _repo;
        public SessionController(IPracticeSessionRepo repo)
        {
            _repo = repo;
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
                _repo.Create(session);
                return RedirectToAction("UserHub", "Home");
            }
            return View(session);
        }

        public IActionResult Edit(int id)
        {
            var session = _repo.GetById(id);
            return View(session);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(PracticeSession session)
        {
            //TODO: Needs ModelState validation, SQL for Update method may need to be adjusted
                _repo.Update(session);
                return RedirectToAction("UserHub", "Home");
        }

    }
}
