﻿using App.Models;
using App.Repo;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

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

        public IActionResult Edit(int id)
        {
            var session = _repo.GetById(id);
            return View(session);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(PracticeSession session)
        {

            _repo.Update(session);
            return RedirectToAction("UserHub", "Home");
        }

        public IActionResult Delete(int id)
        {
            _repo.Delete(id);
            return RedirectToAction("UserHub", "Home");
        }
    }
}
