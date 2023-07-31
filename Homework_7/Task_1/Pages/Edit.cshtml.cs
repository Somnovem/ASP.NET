using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using Task_1.Models;

namespace Task_1.Pages
{
    public class EditModel : PageModel
    {
        ApplicationContext context;
        public User CurrentUser { get; set; }
        public EditModel(ApplicationContext db)
        {
            context = db;
        }
        public void OnGet()
        {
            CurrentUser = context.Users.AsNoTracking().Where(user => user.Username == (string)RouteData.Values["username"]).FirstOrDefault();
        }
        public async Task<IActionResult> OnPostEditAsync(int userId,string newUsername,string oldPassword, string newPassword, DateTime birthdate)
        {
            var userToChange = context.Users.Where(user => user.Id == userId).FirstOrDefault();
            if (userToChange.Password == Encrypter.CalculateSHA256(oldPassword))
            {
                userToChange.Username = newUsername;
                userToChange.Password = Encrypter.CalculateSHA256(newPassword);
                userToChange.BirthDate = birthdate;
                await context.SaveChangesAsync();
                var claims = new List<Claim>
                {
                    new Claim(ClaimTypes.Name, newUsername)
                };
                var identity = new ClaimsIdentity(claims, "LibraryAuthScheme");
                var principal = new ClaimsPrincipal(identity);
                await HttpContext.SignInAsync("LibraryAuthScheme", principal);
                return RedirectToPage("/Users");
        }
            return Page();
        }
    }
}
