namespace Task_1.Models
{
    public class Product
    {
        public int Id { get; set; }
        public string Title { get; set; } = null!;
        public string Description { get; set; } = null!;
        public string Brand { get; set; } = null!;
        public string Category { get; set; } = null!;
        public string Thumbnail { get; set; } = null!;
        public float Rating { get; set; }
        public float Price { get; set; }
        public float DiscountPercentage { get; set; }
        public int Stock { get; set; }
        public List<Review> Reviews { get; set; } = new();
    }

    public class ProductAPIResponse
    {
        public List<Product> products { get; set; }
    }
}
