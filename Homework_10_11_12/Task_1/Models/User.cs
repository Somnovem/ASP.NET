namespace Task_1.Models
{
    public class User
    {
        public Guid Id { get; set; }
        public string Username { get; set; } = null!;
        public string Email { get; set; } = null!;
        public char Gender { get; set; }
        public DateTime BirthDate { get; set; }
        public string Password { get; set; } = null!;
    }
}
