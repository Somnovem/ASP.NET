using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Newtonsoft.Json;
using System.Net.Http;

namespace Task_1.Pages
{
    public class IndexModel : PageModel
    {
        private readonly ILogger<IndexModel> _logger;

        public IndexModel(ILogger<IndexModel> logger)
        {
            _logger = logger;
        }

        public async Task<IActionResult> OnGetSearchBooks(string searchTerm)
        {
            if (string.IsNullOrWhiteSpace(searchTerm))
            {
                return BadRequest("Search term cannot be empty.");
            }

            var apiUrl = $"https://www.googleapis.com/books/v1/volumes?q={searchTerm}";

            using (HttpClient client = new HttpClient())
            {
                var response = await client.GetAsync(apiUrl);

                if (!response.IsSuccessStatusCode)
                {
                    return BadRequest("Error fetching data from the API.");
                }

                var json = await response.Content.ReadAsStringAsync();
                var searchResults = JsonConvert.DeserializeObject<GoogleBooksResponse>(json);

                var books = new List<BookSearchResult>();
                if (searchResults?.Items != null)
                {
                    foreach (var item in searchResults.Items)
                    {
                        var book = new BookSearchResult
                        {
                            Id = item.Id,
                            Title = item.VolumeInfo.Title,
                            Publisher = item.VolumeInfo.Publisher,
                            PublishedDate = item.VolumeInfo.PublishedDate,
                            Authors = item.VolumeInfo.Authors ?? new List<string> { "Unknown Author" },
                            Categories = item.VolumeInfo.Categories ?? new List<string> { "No category listed" },
                            PreviewLink = item.VolumeInfo.PreviewLink,
                            SmallThumbnail = item.VolumeInfo.ImageLinks?.ContainsKey("smallThumbnail") == true ?
                                item.VolumeInfo.ImageLinks["smallThumbnail"] : null
                        };
                        books.Add(book);
                    }
                }

                return new JsonResult(books);
            }

        }
    }

    public class BookSearchResult
    {
        public string Id { get; set; }
        public string Title { get; set; }
        public List<string> Authors { get; set; }
        public string Publisher { get; set; }
        public string PublishedDate { get; set; }
        public List<string> Categories { get; set; }
        public string PreviewLink { get; set; }
        public string SmallThumbnail { get; set; }
    }

    public class GoogleBooksResponse
    {
        public List<GoogleBooksItem> Items { get; set; }
    }

    public class GoogleBooksItem
    {
        public string Id { get; set; }
        public GoogleBooksVolumeInfo VolumeInfo { get; set; }
    }

    public class GoogleBooksVolumeInfo
    {
        public string Id { get; set; }
        public string Title { get; set; }
        public List<string> Authors { get; set; }
        public string Publisher { get; set; }
        public string PublishedDate { get; set; }
        public List<string> Categories { get; set; }
        public Dictionary<string, string> ImageLinks { get; set; }
        public string PreviewLink { get; set; }
    }
}