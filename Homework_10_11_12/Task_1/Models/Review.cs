namespace Task_1.Models
{
    public class Review
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public User? User { get; set; }
        public int ProductId { get; set; }
        public Product? Product { get; set; }
        public string Message { get; set; }
        public int Rating { get; set; }
    }
}
