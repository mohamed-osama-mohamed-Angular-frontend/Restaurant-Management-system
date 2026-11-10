using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Shawermawy.Models
{
    public class CartItem
    {
        [Key]
        public int CartItemId { get; set; }

        public int UserId { get; set; }

        public int ItemId { get; set; }

        public int Quantity { get; set; } = 1;

        [ForeignKey("UserId")]
        public User? User { get; set; }

        [ForeignKey("ItemId")]
        public MenuItem? MenuItem { get; set; }  // ← same fix
    }
}