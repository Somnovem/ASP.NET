using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using Task_1.Models;

namespace Task_1.Pages
{
    public class UsersModel : PageModel
    {
        ApplicationContext context;
        public bool IsLoggedIn { get; set; }
        public User CurrentUser { get; set; }
        public List<string> Usernames { get; set; } = new List<string>();

        public UsersModel(ApplicationContext db)
        {
            context = db;
        }

        public void OnGet()
        {
            var identity = HttpContext.User.Identity as ClaimsIdentity;
            IsLoggedIn = (identity != null && identity.IsAuthenticated);
            if (IsLoggedIn)
            {
                var allUsers = context.Users.AsNoTracking().ToList();
                CurrentUser = allUsers.Where(user => user.Username == (string)identity.FindFirst(ClaimTypes.Name).Value).ToList()[0];
                foreach (var user in allUsers)
                {
                    Usernames.Add(user.Username);
                }
            }
        }

        public async Task<IActionResult> OnPostLoginAsync(string username, string password)
        {
            var user = context.Users.AsNoTracking().Where(user => user.Username == username && user.Password == Encrypter.CalculateSHA256(password)).ToList();
            if (user.Count != 0) {
                var claims = new List<Claim>
                {
                    new Claim(ClaimTypes.Name, username)
                };
                var identity = new ClaimsIdentity(claims, "LibraryAuthScheme");
                var principal = new ClaimsPrincipal(identity);
                await HttpContext.SignInAsync("LibraryAuthScheme", principal);
            }
            return RedirectToPage("/Users");
        }

        public async Task<IActionResult> OnPostRegisterAsync(string username, string email, string password, string gender, DateTime birthday)
        {
            var user = context.Users.Where(user => user.Username == username || user.Email == email).ToList();
            if (user.Count == 0)
            {
                User newUser = new User() { Username = username, Email = email, Password = Encrypter.CalculateSHA256(password), BirthDate = birthday };
                context.Users.Add(newUser);
                context.SaveChanges();
                var claims = new List<Claim>
                {
                    new Claim(ClaimTypes.Name, username)
                };
                var identity = new ClaimsIdentity(claims, "LibraryAuthScheme");
                var principal = new ClaimsPrincipal(identity);
                await HttpContext.SignInAsync("LibraryAuthScheme", principal);
            }
            return RedirectToPage("/Users");
        }

		public async Task<IActionResult> OnGetLogout()
        {
            await HttpContext.SignOutAsync();
            return RedirectToPage("/Users");
        }

		public async Task<IActionResult> OnGetDelete(string username)
		{
            User userToDelete = context.Users.AsNoTracking().Where(user => user.Username == username).ToList()[0];
			context.Users.Remove(userToDelete);
			context.SaveChanges();
			await HttpContext.SignOutAsync();
			return RedirectToPage("/Users");
		}
	}
}
