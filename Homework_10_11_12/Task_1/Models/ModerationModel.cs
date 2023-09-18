namespace Task_1.Models
{
	public class ModerationModel
	{
        public bool IsDeleting { get; set; }
        public List<User> Users { get; set; }
		public Guid Id { get; set; }
		public string Username { get; set; }
		public string Password { get; set; }
		public string? Email { get; set; }
		public string? Gender { get; set; }
		public DateTime? Birthday { get; set; }
	}
}
