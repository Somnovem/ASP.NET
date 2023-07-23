namespace Task_1.Models
{
    public class Book
    {
		public int Id { get; set; }
		public string Title { get; set; } = null!;
        public string Description { get; set; } = null!;
        public string Publisher { get; set; } = null!;
        public string Authors { get; set; } = null!;
        public string Categories { get; set; } = null!;
        public string PageCount { get; set; } = null!;
        public string ThumbnailLink { get; set; } = null!;
    }
}
