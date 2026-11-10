using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Shawermawy.Data;
using Shawermawy.Models;
using Shawermawy.Models.ViewModels;

namespace Shawermawy.Controllers
{
    public class AdminController : Controller
    {
        private readonly AppDbContext _db;
        public AdminController(AppDbContext db) => _db = db;

        private bool IsAdmin() =>
            HttpContext.Session.GetString("IsAdmin") == "True";

        // ── Dashboard ─────────────────────────────────────────────────
        public async Task<IActionResult> Dashboard()
        {
            if (!IsAdmin()) return RedirectToAction("Login", "Account");

            var orders = await _db.Orders
                .Include(o => o.User)
                .Include(o => o.OrderItems).ThenInclude(oi => oi.MenuItem)
                .OrderByDescending(o => o.CreatedAt)
                .ToListAsync();

            return View(orders);
        }

        [HttpPost]
        public async Task<IActionResult> UpdateStatus(int orderId, string status)
        {
            if (!IsAdmin()) return Json(new { success = false });

            var order = await _db.Orders.FindAsync(orderId);
            if (order == null) return Json(new { success = false });

            order.Status = status;
            order.UpdatedAt = DateTime.Now;
            await _db.SaveChangesAsync();
            return Json(new { success = true });
        }

        // ── Reviews ───────────────────────────────────────────────────
        public async Task<IActionResult> Reviews()
        {
            if (!IsAdmin()) return RedirectToAction("Login", "Account");

            var reviews = await _db.Reviews
                .Include(r => r.User)
                .OrderByDescending(r => r.CreatedAt)
                .ToListAsync();

            return View(reviews);
        }

        // ── Menu Management ───────────────────────────────────────────
        public async Task<IActionResult> MenuManager()
        {
            if (!IsAdmin()) return RedirectToAction("Login", "Account");

            var items = await _db.MenuItems
                .AsNoTracking()
                .Include(m => m.Category)
                .OrderBy(m => m.CategoryId)
                .ToListAsync();

            var categories = await _db.Categories
                .AsNoTracking()
                .ToListAsync();

            var vm = new MenuManagerViewModel
            {
                MenuItems = items,
                Categories = categories
            };

            return View(vm);
        }
        [HttpPost]
        public async Task<IActionResult> AddItem(MenuItemFormViewModel form)
        {
            if (!IsAdmin()) return Json(new { success = false });

            if (!ModelState.IsValid)
                return Json(new { success = false, message = "Invalid data." });

            var item = new MenuItem
            {
                CategoryId = form.CategoryId,
                Name = form.Name,
                Description = form.Description,
                Price = form.Price,
                ImageUrl = string.IsNullOrWhiteSpace(form.ImageUrl)
                                  ? "/images/default-food.jpg"
                                  : form.ImageUrl,
                IsAvailable = form.IsAvailable
            };

            _db.MenuItems.Add(item);
            await _db.SaveChangesAsync();

            // Return the new item data so JS can add it to the table instantly
            var category = await _db.Categories.FindAsync(item.CategoryId);
            return Json(new
            {
                success = true,
                itemId = item.ItemId,
                name = item.Name,
                description = item.Description,
                price = item.Price.ToString("F2"),
                categoryName = category?.Name,
                imageUrl = item.ImageUrl,
                isAvailable = item.IsAvailable
            });
        }

        [HttpPost]
        public async Task<IActionResult> EditItem(MenuItemFormViewModel form)
        {
            if (!IsAdmin()) return Json(new { success = false });

            var item = await _db.MenuItems.FindAsync(form.ItemId);
            if (item == null) return Json(new { success = false, message = "Item not found." });

            item.CategoryId = form.CategoryId;
            item.Name = form.Name;
            item.Description = form.Description;
            item.Price = form.Price;
            item.IsAvailable = form.IsAvailable;

            if (!string.IsNullOrWhiteSpace(form.ImageUrl))
                item.ImageUrl = form.ImageUrl;

            await _db.SaveChangesAsync();

            var category = await _db.Categories.FindAsync(item.CategoryId);
            return Json(new
            {
                success = true,
                itemId = item.ItemId,
                name = item.Name,
                description = item.Description,
                price = item.Price.ToString("F2"),
                categoryName = category?.Name,
                imageUrl = item.ImageUrl,
                isAvailable = item.IsAvailable
            });
        }

        [HttpPost]
        public async Task<IActionResult> DeleteItem(int itemId)
        {
            if (!IsAdmin()) return Json(new { success = false });

            var item = await _db.MenuItems.FindAsync(itemId);
            if (item == null) return Json(new { success = false, message = "Item not found." });

            // Soft delete — just mark unavailable so old orders still reference it
            item.IsAvailable = false;
            await _db.SaveChangesAsync();

            return Json(new { success = true });
        }

        [HttpPost]
        public async Task<IActionResult> ToggleAvailability(int itemId)
        {
            if (!IsAdmin()) return Json(new { success = false });

            var item = await _db.MenuItems.FindAsync(itemId);
            if (item == null) return Json(new { success = false });

            item.IsAvailable = !item.IsAvailable;
            await _db.SaveChangesAsync();

            return Json(new { success = true, isAvailable = item.IsAvailable });
        }

        [HttpPost]
        public async Task<IActionResult> AddCategory(string name, string iconClass)
        {
            if (!IsAdmin()) return Json(new { success = false });

            if (string.IsNullOrWhiteSpace(name))
                return Json(new { success = false, message = "Category name required." });

            var cat = new Category
            {
                Name = name,
                IconClass = string.IsNullOrWhiteSpace(iconClass) ? "fa-utensils" : iconClass
            };

            _db.Categories.Add(cat);
            await _db.SaveChangesAsync();

            return Json(new { success = true, categoryId = cat.CategoryId, name = cat.Name });
        }
    }
}