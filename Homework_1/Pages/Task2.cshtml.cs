using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Homework_1.Pages
{
    public class Task2Model : PageModel
    {
        public string Message { get; set; }
        public void OnGet()
        {
            Message = "Hello, World!";
		}
    }
}
