using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using Task_1.Models;

namespace Task_1.Pages
{
    public class MessagesModel : PageModel
    {
        private ApplicationContext context;

        public string Username { get; set; }
        public bool IsManager { get; set; }

        public List<UserMessage> userMessages { get; set; }

        public MessagesModel(ApplicationContext db)
        {
            context = db;
        }

        public IActionResult OnGet()
        {
            var identity = HttpContext.User.Identity as ClaimsIdentity;
            if (identity != null && identity.IsAuthenticated)
            {
                Username = identity.FindFirst(ClaimTypes.Name)?.Value;
                bool.TryParse(Request.Cookies["IsManager"], out bool isManagerValue);
                IsManager = isManagerValue;
                if (IsManager)
                {
                    userMessages = context.UserMessages.AsNoTracking().ToList();
                }
                return Page();
            }
            else
            {
                return RedirectToPage("/Authorisation");
            }
        }

        public async Task<IActionResult> OnPost(string username, string message){
            try
            {
                context.UserMessages.Add(new UserMessage
                {
                    UserId = context.Users.Where(user => user.Username == username).ToList()[0].Id,
                    MessageText = message,
                    DateSent = DateTime.Now
                });
                context.SaveChanges();
                return RedirectToPage("/Store");
            }
            catch
            {
                return Page();
            }
        }
    }
}
