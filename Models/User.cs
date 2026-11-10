using Microsoft.AspNetCore.Mvc.ViewEngines;

namespace Shawermawy.Models
{
    public class User
    {
        public int UserId { get; set; }
        public string Username { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string PhoneNumber { get; set; } = string.Empty;
        public string PasswordHash { get; set; } = string.Empty;
        public bool IsAdmin { get; set; } = false;
        public DateTime CreatedAt { get; set; } = DateTime.Now;

        public ICollection<CartItem> CartItems { get; set; } = new List<CartItem>();
        public ICollection<Order> Orders { get; set; } = new List<Order>();
        public ICollection<Review> Reviews { get; set; } = new List<Review>();
    }
}