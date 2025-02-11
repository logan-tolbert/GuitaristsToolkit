﻿using App.Data.Context;
using App.Models;
using App.Repo;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;

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

        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Create(Setlist setlist)
        {
            setlist.UserId = 1;
            setlist.CreatedAt = DateTime.UtcNow;

            //TODO: Implement song repo
            //ViewBag.Songs = _songRepo.GetAll();
            int setListId = _repo.Create(setlist);

            return RedirectToAction("Edit", new { id = setListId });
        }


        [HttpGet]
        public IActionResult GetUserSetlists()
        {
            int userId = 1;
            var setlists = _repo.GetSetlistsForUser(userId);

            Console.WriteLine("Setlist Results: " + JsonSerializer.Serialize(setlists));
            return Json(setlists);
        }

        [HttpGet]
        public IActionResult Details(int id)
        {
            var setlist = _repo.GetSetlistWithSongs(id);
            return View(setlist);
        }

        [HttpGet]
        public IActionResult Edit(int id)
        {
            var setlist = _repo.GetSetlistWithSongs(id);
            //TODO: Implement song repo
            //ViewBag.Songs = _songRepo.GetAll();
            return View(setlist);
        }


    }
}
