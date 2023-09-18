namespace Task_1.Models
{
    public class PersonLogin
    {
        public bool IsLoggedIn { get; set; }
        public bool IsLoggingOut { get; set; }
        public bool IsLogin { get; set; }
        public bool IsManager { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public string? Email { get; set; }
        public string? Gender { get; set; }
        public DateTime? Birthday { get; set; }
    }
}
