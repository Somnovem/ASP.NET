namespace Task_1.Models
{
    public class Manager
    {
        public int Id { get; set; }
        public string Username { get; set; } = null!;
        public string FirstName { get; set; } = null!;
        public string LastName { get; set; } = null!;
        public string Email { get; set; } = null!;
        public char Gender { get; set; }
        public DateTime BirthDate { get; set; }
        public DateTime EmployedDate { get; set; }
        public string Password { get; set; } = null!;
    }
}
