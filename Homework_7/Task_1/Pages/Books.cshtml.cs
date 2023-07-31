using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Task_1.Models;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System.Linq;

namespace Task_1.Pages
{
    public class BooksModel : PageModel
    {
        ApplicationContext context;
        public List<Book> Books { get; private set; } = new();
        public List<string> Categories { get; private set; } = new();
        public BooksModel(ApplicationContext db)
        {
            context = db;
        }
        public async Task<IActionResult> OnGet()
        {
            Books = context.Books.AsNoTracking().ToList();
            if (Books.Count == 0)
            {
                var apiUrl = $"https://www.googleapis.com/books/v1/volumes?q=LOTR";
                using (HttpClient client = new HttpClient())
                {
                    var response = await client.GetAsync(apiUrl);

                    if (!response.IsSuccessStatusCode)
                    {
                        return BadRequest("Error fetching data from the API.");
                    }

                    var json = await response.Content.ReadAsStringAsync();
                    var searchResults = JsonConvert.DeserializeObject<GoogleBooksResponse>(json);
                    if (searchResults?.Items != null)
                    {
                        foreach (var item in searchResults.Items)
                        {
                            var book = new Book
                            {
                                Title = item.VolumeInfo.Title,
                                Publisher = item.VolumeInfo.Publisher,
                                PublishedDate = item.VolumeInfo.PublishedDate,
                                Description = item.VolumeInfo.Description,
                                PageCount = item.VolumeInfo.PageCount,
                                AverageRating = item.VolumeInfo.AverageRating,
                                RatingsCount = item.VolumeInfo.RatingsCount,
                                Authors = item.VolumeInfo.Authors.ConcatenateWithSeparator(", "),
                                Categories = item.VolumeInfo.Categories.ConcatenateWithSeparator(", "),
                                ThumbnailLink = item.VolumeInfo.ImageLinks?.ContainsKey("smallThumbnail") == true ?
                                    item.VolumeInfo.ImageLinks["smallThumbnail"] : null
                            };
							context.Books.Add(book);
                        }
						context.SaveChanges();  
                        Books = context.Books.AsNoTracking().ToList();
					}
                }
            }
			for (int i = 0; i < Books.Count; i++)
			{
                var splitCategories = Books[i].Categories.Split(", ");
                for (int j = 0; j < splitCategories.Length; j++)
                {
					if (Categories.Contains(splitCategories[j])) continue;
					Categories.Add(splitCategories[j]);
				}
			}
			return Page();
        }

		public IActionResult OnGetJsonData()
		{
			var jsonData = JsonConvert.SerializeObject(context.Books.AsNoTracking().ToList());
			return new JsonResult(jsonData);
		}
	}

    public class GoogleBooksResponse  
    {
        public List<GoogleBooksItem> Items { get; set; } 
    }

    public class GoogleBooksItem
    {
		public GoogleBooksVolumeInfo VolumeInfo { get; set; } 
    }

    public class GoogleBooksVolumeInfo
    {
        public string Title { get; set; } 
        public List<string> Authors { get; set; }
        public string Publisher { get; set; }
        public string PublishedDate { get; set; }
        public string Description { get; set; } 
        public int PageCount { get; set; }
		public float AverageRating { get; set; }
		public int RatingsCount { get; set; }
		public List<string> Categories { get; set; } 
        public Dictionary<string, string> ImageLinks { get; set; } 
    }
}
