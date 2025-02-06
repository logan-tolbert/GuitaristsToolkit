using Microsoft.AspNetCore.Mvc;

namespace PracticeTracker.Controllers
{
    public class SessionController : Controller
    {
        //GET: Session/Create
        public IActionResult Create()
        {
            return View();
        }
    }
}
