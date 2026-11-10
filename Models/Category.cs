namespace Shawermawy.Models
{
    public class Category
    {
        public int CategoryId { get; set; }
        public string Name { get; set; } = string.Empty;
        public string? IconClass { get; set; }

        public ICollection<MenuItem> MenuItems { get; set; } = new List<MenuItem>();
    }
}