using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Newtonsoft.Json;
using System.Text.RegularExpressions;

namespace Task_1.Pages
{
    public class DetailsModel : PageModel
    {
        [BindProperty]
        public BookDetails Details { get; set; }

        public async Task<IActionResult> OnGet()
        {
            var bookId = (string)RouteData.Values["id"];
            if (!new Regex("^[a-zA-Z0-9_-]{12}$").IsMatch(bookId))
            {
                return NotFound();
            }
            Details = await GetBookDetails(bookId);

            if (Details == null)
            {
                return NotFound();
            }
            Details.Description = Details.Description.Replace("<p>", "");
            Details.Description = Details.Description.Replace("</p>", "");
            return Page();
        }

        private async Task<BookDetails> GetBookDetails(string id)
        {
            using (HttpClient client = new HttpClient())
            {
                var apiUrl = $"https://www.googleapis.com/books/v1/volumes/{id}";
                var response = await client.GetAsync(apiUrl);

                if (!response.IsSuccessStatusCode)
                {
                    return null;
                }

                var json = await response.Content.ReadAsStringAsync();
                var volumeInfo = JsonConvert.DeserializeObject<BookDetailsVolumeInfo>(json);
                return volumeInfo?.VolumeInfo;
            }
        }
    }

    public class BookDetailsVolumeInfo
    {
        public BookDetails VolumeInfo { get; set; }
    }

    public class BookDetails
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public List<string> Authors { get; set; }
        public string Publisher { get; set; }
        public string PageCount { get; set; }
        public string AverageRating { get; set; }
        public string PublishedDate { get; set; }
        public List<string> Categories { get; set; }
        public Dictionary<string, string> ImageLinks { get; set; }
    }
}
