using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Shawermawy.Data;
using Shawermawy.Models;
using Shawermawy.Models.ViewModels;

namespace Shawermawy.Controllers
{
    public class AccountController : Controller
    {
        private readonly AppDbContext _db;
        public AccountController(AppDbContext db) => _db = db;

        public IActionResult Login() => View();

        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            if (!ModelState.IsValid) return View(model);

            var user = await _db.Users.FirstOrDefaultAsync(u =>
                (u.Username == model.UsernameOrEmail || u.Email == model.UsernameOrEmail)
                && u.PasswordHash == model.Password);

            if (user == null)
            {
                ModelState.AddModelError("", "Invalid credentials. Please try again.");
                return View(model);
            }

            HttpContext.Session.SetInt32("UserId", user.UserId);
            HttpContext.Session.SetString("Username", user.Username);
            HttpContext.Session.SetString("IsAdmin", user.IsAdmin.ToString());

            return user.IsAdmin
                ? RedirectToAction("Dashboard", "Admin")
                : RedirectToAction("Index", "Menu");
        }

        public IActionResult Register() => View();

        [HttpPost]
        public async Task<IActionResult> Register(RegisterViewModel model)
        {
            if (!ModelState.IsValid) return View(model);

            bool exists = await _db.Users.AnyAsync(u =>
                u.Username == model.Username || u.Email == model.Email);

            if (exists)
            {
                ModelState.AddModelError("", "Username or Email already exists.");
                return View(model);
            }

            var user = new User
            {
                Username = model.Username,
                Email = model.Email,
                PhoneNumber = model.PhoneNumber,
                PasswordHash = model.Password
            };

            _db.Users.Add(user);
            await _db.SaveChangesAsync();

            TempData["Success"] = "Registration successful! Please log in.";
            return RedirectToAction("Login");
        }

        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            return RedirectToAction("Login");
        }
    }
}