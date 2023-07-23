namespace Task_1.Models
{
    public class UserMessage
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public string MessageText { get; set; } = null!;
        public DateTime DateSent { get; set; }
    }
}
