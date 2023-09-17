using System.ComponentModel.DataAnnotations;
namespace Homework_22_REST.Models;


public class User {
    public int Id { get; set; }

    [Required]
    public string Firstname { get; set; }

    [Required]
    public string Lastname { get; set; }

    [Required]
    public DateTime Birthday { get; set; }

}
