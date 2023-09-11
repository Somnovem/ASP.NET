using System.ComponentModel.DataAnnotations;

namespace Practice_20.ViewModels
{
    public class LoginModel
    {
        [Required(ErrorMessage = "Email required")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Password required")]
        [DataType(DataType.Password)]
        public string Password { get; set; }
    }
}
