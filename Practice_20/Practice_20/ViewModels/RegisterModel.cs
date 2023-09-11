using System.ComponentModel.DataAnnotations;

namespace Practice_20.ViewModels
{
    public class RegisterModel
    {
        [Required(ErrorMessage = "Email required")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Password required")]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Required(ErrorMessage = "Birthday required")]
        [DataType(DataType.Date)]
        public DateTime Birthday { get; set; }

        [DataType(DataType.Password)]
        [Compare("Password", ErrorMessage = "Passwords dont match")]
        public string ConfirmPassword { get; set; }
    }
}
