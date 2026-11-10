using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Shawermawy.Data;
using Shawermawy.Models;
using Shawermawy.Models.ViewModels;

namespace Shawermawy.Controllers
{
    public class CartController : Controller
    {
        private readonly AppDbContext _db;
        public CartController(AppDbContext db) => _db = db;

        private int? GetUserId() => HttpContext.Session.GetInt32("UserId");

        public async Task<IActionResult> Index()
        {
            var userId = GetUserId();
            if (userId == null) return RedirectToAction("Login", "Account");

            var items = await _db.CartItems
                .Include(c => c.MenuItem)
                .Where(c => c.UserId == userId)
                .ToListAsync();

            return View(new CartViewModel { CartItems = items });
        }

        [HttpPost]
        public async Task<IActionResult> Add(int itemId)
        {
            var userId = GetUserId();
            if (userId == null) return Json(new { success = false });

            var existing = await _db.CartItems
                .FirstOrDefaultAsync(c => c.UserId == userId && c.ItemId == itemId);

            if (existing != null)
                existing.Quantity++;
            else
                _db.CartItems.Add(new CartItem { UserId = userId.Value, ItemId = itemId });

            await _db.SaveChangesAsync();
            var count = await _db.CartItems.Where(c => c.UserId == userId).SumAsync(c => c.Quantity);
            return Json(new { success = true, count });
        }

        [HttpPost]
        public async Task<IActionResult> UpdateQuantity(int cartItemId, int quantity)
        {
            var item = await _db.CartItems.FindAsync(cartItemId);
            if (item == null) return Json(new { success = false });

            if (quantity <= 0)
                _db.CartItems.Remove(item);
            else
                item.Quantity = quantity;

            await _db.SaveChangesAsync();
            return Json(new { success = true });
        }

        [HttpPost]
        public async Task<IActionResult> Remove(int cartItemId)
        {
            var item = await _db.CartItems.FindAsync(cartItemId);
            if (item != null) _db.CartItems.Remove(item);
            await _db.SaveChangesAsync();
            return Json(new { success = true });
        }

        [HttpPost]
        public async Task<IActionResult> Clear()
        {
            var userId = GetUserId();
            if (userId == null) return Json(new { success = false });

            var items = _db.CartItems.Where(c => c.UserId == userId);
            _db.CartItems.RemoveRange(items);
            await _db.SaveChangesAsync();
            return Json(new { success = true });
        }
        public async Task<IActionResult> GetCount()
        {
            var userId = GetUserId();
            if (userId == null) return Json(new { count = 0 });

            var count = await _db.CartItems
                .Where(c => c.UserId == userId)
                .SumAsync(c => c.Quantity);

            return Json(new { count });
        }

        [HttpPost]
        public async Task<IActionResult> Checkout(string paymentMethod)
        {
            var userId = GetUserId();
            if (userId == null) return RedirectToAction("Login", "Account");

            var cartItems = await _db.CartItems
                .Include(c => c.MenuItem)
                .Where(c => c.UserId == userId)
                .ToListAsync();

            if (!cartItems.Any())
            {
                TempData["Error"] = "Your cart is empty.";
                return RedirectToAction("Index");
            }

            var order = new Order
            {
                UserId = userId.Value,
                PaymentMethod = paymentMethod,
                TotalAmount = cartItems.Sum(c => c.MenuItem!.Price * c.Quantity),
                Status = "OnHold"
            };

            _db.Orders.Add(order);
            await _db.SaveChangesAsync();

            foreach (var ci in cartItems)
            {
                _db.OrderItems.Add(new OrderItem
                {
                    OrderId = order.OrderId,
                    ItemId = ci.ItemId,
                    Quantity = ci.Quantity,
                    UnitPrice = ci.MenuItem!.Price
                });
            }

            _db.CartItems.RemoveRange(cartItems);
            await _db.SaveChangesAsync();

            TempData["Success"] = "Order placed successfully!";
            return RedirectToAction("Index", "Order");
        }
    }
}