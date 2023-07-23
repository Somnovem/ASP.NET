namespace Task_1.Models
{
    public class ProductFeedback
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public int ProductId { get; set; }
        public string MessageText { get; set; } = null!;
        public DateTime DateSent { get; set; }
    }
}
