namespace Task_1.Models
{
    public class Product
    {
        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public string Description { get; set; } = null!;
        public string Brand { get; set; } = null!;
        public string Category { get; set; } = null!;
        public string ThumbnailLink { get; set; } = null!;
        public float Rating { get; set; }
        public float Price { get; set; }
        public float DiscountPercentage { get; set; }
        public int Stock { get; set; }
    }
}
