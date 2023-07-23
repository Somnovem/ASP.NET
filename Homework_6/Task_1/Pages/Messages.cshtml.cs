using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Security.Claims;

namespace Task_1.Pages
{
    public class MessagesModel : PageModel
    {
        public string Username { get; set; }

        public IActionResult OnGet()
        {
            var identity = HttpContext.User.Identity as ClaimsIdentity;
            if (identity != null && identity.IsAuthenticated)
            {
                Username = identity.FindFirst(ClaimTypes.Name)?.Value;
                bool isManager = bool.TryParse(Request.Cookies["IsManager"], out bool isManagerValue);
                if (isManager && isManagerValue)
                {
                    Username += "- Manager";
                }
                return Page();
            }
            else
            {
                return RedirectToPage("/Authorisation");
            }
        }
    }
}
