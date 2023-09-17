using System.ComponentModel.DataAnnotations;
namespace Test.Models;


public class User {
    public int Id { get; set; }

    [Required]
    public string Firstname { get; set; }

    [Required]
    public string Lastname { get; set; }

    [Required]
    public DateTime Birthday { get; set; }

}
