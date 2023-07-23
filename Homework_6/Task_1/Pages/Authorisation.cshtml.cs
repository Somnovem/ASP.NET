using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using Task_1.Models;

namespace Task_1.Pages
{
    public class AuthorisationModel : PageModel
    {
        ApplicationContext context;
        public bool IsLoggedIn { get; set; }
        public string Username { get; set; }
        public AuthorisationModel(ApplicationContext db)
        {
            context = db;
        }
        public void OnGet()
        {
            var identity = HttpContext.User.Identity as ClaimsIdentity;
            IsLoggedIn = (identity != null && identity.IsAuthenticated);
            Username = identity.FindFirst(ClaimTypes.Name)?.Value;
        }

        public async Task<IActionResult> OnPostLoginAsync(string username, string password)
        {
            var user = context.Users.AsNoTracking().Where(user => user.Username == username && user.Password == Encrypter.CalculateSHA256(password)).ToList();
            if (user.Count == 0)
            {
                var manager = context.Managers.AsNoTracking().Where(manager => manager.Username == username && manager.Password == Encrypter.CalculateSHA256(password)).ToList();
                if (manager.Count != 0)
                {
                    Response.Cookies.Append("IsManager", "True");
                }
                else {
                    Response.Cookies.Append("IsManager", "False");
                    return Page();
                }
            }

            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, username)
            };

            var identity = new ClaimsIdentity(claims, "LoremIpsumAuthScheme");
            var principal = new ClaimsPrincipal(identity);
            await HttpContext.SignInAsync("LoremIpsumAuthScheme", principal);
            return RedirectToPage("/Messages");
        }
        public async Task<IActionResult> OnPostRegisterAsync(string username, string email,string password,string gender,DateTime birthday)
        {
            var user = context.Users.Where(user => user.Username == username).ToList();
            if (user.Count != 0) return Page();

            User newUser = new User() {Username = username, Email = email, Password = Encrypter.CalculateSHA256(password), Gender = gender[0], BirthDate = birthday};
            context.Users.Add(newUser);
            context.SaveChanges();

            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, username)
            };

            var identity = new ClaimsIdentity(claims, "LoremIpsumAuthScheme");
            var principal = new ClaimsPrincipal(identity);
            await HttpContext.SignInAsync("LoremIpsumAuthScheme", principal);
            return RedirectToPage("/Messages");
        }
    }
}
