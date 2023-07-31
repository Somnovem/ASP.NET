using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Task_1.Models;

namespace Task_1.Pages
{
    public class DetailsModel : PageModel
    {
        ApplicationContext context;
        public Book Book { get; set; }
        public DetailsModel(ApplicationContext db)
        {
            context = db;
        }
        public void OnGet()
        {
            Book = context.Books.AsNoTracking().Where(product => product.Id == Int32.Parse((string)RouteData.Values["id"])).ToList()[0];
        }
    }
}
