namespace Shawermawy.Models
{
    public class Order
    {
        public int OrderId { get; set; }
        public int UserId { get; set; }
        public string Status { get; set; } = "OnHold";
        public string? PaymentMethod { get; set; }
        public decimal TotalAmount { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.Now;
        public DateTime UpdatedAt { get; set; } = DateTime.Now;

        public User? User { get; set; }
        public ICollection<OrderItem> OrderItems { get; set; } = new List<OrderItem>();
    }
}