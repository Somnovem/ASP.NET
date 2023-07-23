using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Task_1.Models;
using Newtonsoft.Json;
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
        public void OnGet()
        {
            Products = context.Products.AsNoTracking().ToList();
            Products.ForEach(p => {
                if (!Categories.Contains(p.Category))Categories.Add(p.Category);
            });
        }

        public IActionResult OnGetJsonData() {
            var jsonData = JsonConvert.SerializeObject(context.Products.AsNoTracking().ToList());
            return new JsonResult(jsonData);
        }
    }
}
