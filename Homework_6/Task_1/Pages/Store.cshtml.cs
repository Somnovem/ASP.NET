using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Task_1.Models;
using Newtonsoft.Json;
using static System.Reflection.Metadata.BlobBuilder;

namespace Task_1.Pages
{
    public class StoreModel : PageModel
    {
        ApplicationContext context;
        public List<Product> Products { get; private set; } = new();
        [BindProperty]
        public List<string> Categories { get; private set; } = new();

        public StoreModel(ApplicationContext db)
        {
            context = db;
        }
        public async Task<IActionResult> OnGet()
        {
            Products = context.Products.AsNoTracking().ToList();
            if (Products.Count == 0)
            {
                var apiUrl = $"https://dummyjson.com/products";
                using (HttpClient client = new HttpClient())
                {
                    var response = await client.GetAsync(apiUrl);

                    if (!response.IsSuccessStatusCode)
                    {
                        return BadRequest("Error fetching data from the API.");
                    }

                    var json = await response.Content.ReadAsStringAsync();
                    var searchResults = JsonConvert.DeserializeObject<ProductAPIResponse>(json);
                    if (searchResults?.products != null)
                    {
                        foreach (var item in searchResults.products)
                        {
                            Products.Add(new Product()
                            {
                                Title = item.Title,
                                Price = item.Price,
                                Rating = item.Rating,
                                Stock = item.Stock,
                                Brand = item.Brand,
                                Category = item.Category,
                                Description = item.Description,
                                DiscountPercentage = item.DiscountPercentage,
                                Thumbnail = item.Thumbnail
                            });
                        }
                        context.Products.AddRange(Products);
                        context.SaveChanges();
                    }
                }
            }
            Products.ForEach(p => {
                if (!Categories.Contains(p.Category))Categories.Add(p.Category);
            });
            return Page();
        }

        public IActionResult OnGetJsonData() {
            var jsonData = JsonConvert.SerializeObject(context.Products.AsNoTracking().ToList());
            return new JsonResult(jsonData);
        }
    }

    public class ProductAPIResponse
    {
        public List<Product> products { get; set; }
    }
}
