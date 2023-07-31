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

        public async Task<IActionResult> OnPost(string bookId, string title,string description,int pageCount,string rating, int ratingsCount,string publisher, string publishedDate,string categories,string authors)
        {
            if (string.IsNullOrEmpty(bookId))
            {
                //new book
                context.Books.Add(new Book()
                {
                Title = title,
                Description = description,
                PageCount = pageCount,
                AverageRating = float.Parse(rating, System.Globalization.CultureInfo.InvariantCulture),
                RatingsCount = ratingsCount,
                Publisher = publisher,
                PublishedDate = publishedDate,
                Categories = categories,
                Authors = authors
                });
                await context.SaveChangesAsync();
            }
            else
            {
                //edit book
                int id = Int32.Parse(bookId);
                Book bookToChange = context.Books.Where(book => book.Id == id).FirstOrDefault();
                bookToChange.Title = title;
                bookToChange.Description = description;
                bookToChange.PageCount = pageCount;
                bookToChange.AverageRating = float.Parse(rating, System.Globalization.CultureInfo.InvariantCulture);
                bookToChange.RatingsCount = ratingsCount;
                bookToChange.Publisher = publisher;
                bookToChange.PublishedDate = publishedDate;
                bookToChange.Categories = categories;
                bookToChange.Authors = authors;
                await context.SaveChangesAsync();
            }
            return RedirectToPage("/EditBooks");
        }

        public IActionResult OnGetJsonData()
        {
            var jsonData = JsonConvert.SerializeObject(context.Books.AsNoTracking().ToList());
            return new JsonResult(jsonData);
        }

        public IActionResult OnGetDelete(int id)
        {
            Book bookToDelete = context.Books.Where(book => book.Id == id).FirstOrDefault();
            context.Books.Remove(bookToDelete);
            context.SaveChanges();
            return RedirectToPage("/EditBooks");
        }
    }
}
