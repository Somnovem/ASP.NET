using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using Task_1.Models;

namespace Task_1.Pages
{
    public class EditBooksModel : PageModel
    {
		ApplicationContext context;
		public List<Book> Books { get; private set; } = new();
		public EditBooksModel(ApplicationContext db)
		{
			context = db;
		}
		public void OnGet()
        {
			Books = context.Books.AsNoTracking().ToList();
		}

        public async Task<IActionResult> OnPost(string bookId, string title)
        {
            Console.WriteLine(bookId);
            Console.WriteLine(title);
            return RedirectToPage("/EditBooks");
        }

        public IActionResult OnGetJsonData()
        {
            var jsonData = JsonConvert.SerializeObject(context.Books.AsNoTracking().ToList());
            return new JsonResult(jsonData);
        }
    }
}
