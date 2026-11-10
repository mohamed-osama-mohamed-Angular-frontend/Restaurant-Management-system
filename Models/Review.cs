namespace Shawermawy.Models
{
    public class Review
    {
        public int ReviewId { get; set; }
        public int UserId { get; set; }
        public string Title { get; set; } = string.Empty;
        public string Message { get; set; } = string.Empty;
        public string Type { get; set; } = "Review";
        public DateTime CreatedAt { get; set; } = DateTime.Now;

        public User? User { get; set; }
    }
}