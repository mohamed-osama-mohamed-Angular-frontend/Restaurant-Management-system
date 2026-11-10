using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Shawermawy.Data;
using Shawermawy.Models.ViewModels;

namespace Shawermawy.Controllers
{
    public class OrderController : Controller
    {
        private readonly AppDbContext _db;
        public OrderController(AppDbContext db) => _db = db;

        public async Task<IActionResult> Index()
        {
            var userId = HttpContext.Session.GetInt32("UserId");
            if (userId == null) return RedirectToAction("Login", "Account");

            var orders = await _db.Orders
                .Include(o => o.OrderItems)
                .ThenInclude(oi => oi.MenuItem)
                .Where(o => o.UserId == userId)
                .OrderByDescending(o => o.CreatedAt)
                .ToListAsync();

            var viewModels = orders.Select(o => new OrderViewModel
            {
                Order = o,
                TimeRemaining = o.Status == "OnHold"
                    ? TimeSpan.FromMinutes(10) - (DateTime.Now - o.CreatedAt)
                    : null
            }).ToList();

            return View(viewModels);
        }

        [HttpPost]
        public async Task<IActionResult> Cancel(int orderId)
        {
            var userId = HttpContext.Session.GetInt32("UserId");
            var order = await _db.Orders.FirstOrDefaultAsync(o =>
                o.OrderId == orderId && o.UserId == userId && o.Status == "OnHold");

            if (order == null)
                return Json(new { success = false, message = "Cannot cancel this order." });

            if ((DateTime.Now - order.CreatedAt).TotalMinutes > 10)
                return Json(new { success = false, message = "Cancellation window has expired." });

            _db.Orders.Remove(order);
            await _db.SaveChangesAsync();
            return Json(new { success = true });
        }
    }
}