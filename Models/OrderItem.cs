using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Shawermawy.Models
{
    public class OrderItem
    {
        [Key]
        public int OrderItemId { get; set; }

        public int OrderId { get; set; }

        public int ItemId { get; set; }

        public int Quantity { get; set; }

        public decimal UnitPrice { get; set; }

        [ForeignKey("OrderId")]
        public Order? Order { get; set; }

        [ForeignKey("ItemId")]
        public MenuItem? MenuItem { get; set; }  // ← tells EF: FK is ItemId, not MenuItemItemId
    }
}