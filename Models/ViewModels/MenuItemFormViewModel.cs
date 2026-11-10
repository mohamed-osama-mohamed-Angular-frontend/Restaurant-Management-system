using System.ComponentModel.DataAnnotations;

namespace Shawermawy.Models.ViewModels
{
    public class MenuItemFormViewModel
    {
        public int ItemId { get; set; }  // 0 = new item

        [Required]
        public int CategoryId { get; set; }

        [Required]
        [StringLength(100)]
        public string Name { get; set; } = string.Empty;

        [StringLength(500)]
        public string? Description { get; set; }

        [Required]
        [Range(0.01, 10000)]
        public decimal Price { get; set; }

        public string? ImageUrl { get; set; }

        public bool IsAvailable { get; set; } = true;
    }
}