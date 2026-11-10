using Microsoft.AspNetCore.Mvc;
using Shawermawy.Data;
using Shawermawy.Models;

namespace Shawermawy.Controllers
{
    public class ReviewController : Controller
    {
        private readonly AppDbContext _db;
        public ReviewController(AppDbContext db) => _db = db;

        public IActionResult Index()
        {
            if (HttpContext.Session.GetInt32("UserId") == null)
                return RedirectToAction("Login", "Account");
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Submit(string title, string message, string type)
        {
            var userId = HttpContext.Session.GetInt32("UserId");
            if (userId == null) return Json(new { success = false });

            _db.Reviews.Add(new Review
            {
                UserId = userId.Value,
                Title = title,
                Message = message,
                Type = type
            });

            await _db.SaveChangesAsync();
            return Json(new { success = true });
        }
    }
}