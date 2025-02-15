using App.Models;
using App.Services;
using GuitaristsToolkit.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace GuitaristsToolkit.Controllers
{
    public class UserController : Controller
    {
        private readonly UserAuthenticationService _authService;
        private readonly UserRegistrationService _registrationService;

        public UserController(UserAuthenticationService authService, UserRegistrationService registrationService)
        {
            _authService = authService;
            _registrationService = registrationService;
        }

        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Login(LoginRequestViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            if (_authService.AuthenticateUser(model.EmailOrUsername, model.Password, out var user))
            {
                HttpContext.Session.SetString("UserId", user.Id.ToString());
                return RedirectToAction("UserHub", "Home");
            }

            ViewData["Error"] = "Invalid credentials. Please try again.";
            return View(model);
        }

        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Register(RegisterViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            bool isRegistered = _registrationService.RegisterUser(model.Username, model.FirstName, model.LastName, model.Email, model.Password, "Default");
            if (isRegistered)
            {
                return RedirectToAction("Login");
            }

            ViewData["Error"] = "Registration failed. Username or Email might already be taken.";
            return View(model);
        }

        [HttpPost]
        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            return RedirectToAction("Login");
        }
    }
}
