using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Shawermawy.Data;

namespace Shawermawy.Controllers
{
    public class MenuController : Controller
    {
        private readonly AppDbContext _db;
        public MenuController(AppDbContext db) => _db = db;

        public async Task<IActionResult> Index()
        {
            if (HttpContext.Session.GetInt32("UserId") == null)
                return RedirectToAction("Login", "Account");

            var categories = await _db.Categories
                .Include(c => c.MenuItems.Where(m => m.IsAvailable))
                .ToListAsync();

            return View(categories);
        }
    }
}