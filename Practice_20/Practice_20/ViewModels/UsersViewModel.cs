using Practice_20.Models;

namespace Practice_20.ViewModels;

public class UsersViewModel
{
    public List<User> Users { get; set; } = new();
    public List<Role> Roles { get; set; } = new();

    public User User { get; set; } = new();
    public FormAction Action { get; set; }
    
}