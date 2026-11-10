namespace Shawermawy.Models.ViewModels
{
    public class MenuManagerViewModel
    {
        public List<MenuItem> MenuItems { get; set; } = new();
        public List<Category> Categories { get; set; } = new();
    }
}