﻿namespace Task_1.Models
{
    public class User
    {
        public int Id { get; set; }
        public string Username { get; set; } = null!;
        public DateTime BirthDate { get; set; }
        public string Email { get; set; } = null!;
        public string Password { get; set; } = null!;
    }
}
