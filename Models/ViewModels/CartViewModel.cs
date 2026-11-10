namespace Shawermawy.Models.ViewModels
{
    public class CartViewModel
    {
        public List<CartItem> CartItems { get; set; } = new();
        public decimal Total => CartItems.Sum(x => (x.MenuItem?.Price ?? 0) * x.Quantity);
    }
}