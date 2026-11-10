namespace Shawermawy.Models.ViewModels
{
    public class OrderViewModel
    {
        public Order Order { get; set; } = new();
        public TimeSpan? TimeRemaining { get; set; }
        public bool CanCancel => TimeRemaining.HasValue && TimeRemaining.Value.TotalSeconds > 0;
    }
}