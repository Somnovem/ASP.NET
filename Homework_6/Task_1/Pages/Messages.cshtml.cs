using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Security.Claims;
using Task_1.Models;

namespace Task_1.Pages
{
    public class MessagesModel : PageModel
    {
        private ApplicationContext context;

        public string Username { get; set; }
        public bool IsManager { get; set; }

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
                if (isManagerValue)
                {
                    Username += " - Manager";
                }
                return Page();
            }
            else
            {
                return RedirectToPage("/Authorisation");
            }
        }

        public async Task<IActionResult> OnPostAsync(string username, string message)
        {
            try
            {
                context.UserMessages.Add(new UserMessage
                {
                    UserId = context.Users.Where(user => user.Username == username).ToList()[0].Id,
                    MessageText = message,
                    DateSent = DateTime.Now
                });
                return new JsonResult(new { Success = true, Message = "Message sent successfully." });
            }
            catch
            {
                return new JsonResult(new { Success = false, Message = "Error encountered while sending the message." });
            }
        }
    }

    public class MessageData
    {
        public string Username { get; set; }
        public string Message { get; set; }
    }
}
