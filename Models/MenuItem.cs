using System.ComponentModel.DataAnnotations;

namespace Shawermawy.Models
{
    public class MenuItem
    {
        [Key]
        public int ItemId { get; set; }
        public int CategoryId { get; set; }
        public string Name { get; set; } = string.Empty;
        public string? Description { get; set; }
        public decimal Price { get; set; }
        public string? ImageUrl { get; set; }
        public bool IsAvailable { get; set; } = true;

        public Category? Category { get; set; }
    }
}