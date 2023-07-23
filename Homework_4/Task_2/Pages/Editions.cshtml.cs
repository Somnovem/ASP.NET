using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Newtonsoft.Json;
namespace Task_2.Pages
{
    public class EditionsModel : PageModel
    {
        [BindProperty]
        public Edition[] Editions { get; set; } = null;
        public void OnGet()
        {
            string pathToJson = Directory.GetCurrentDirectory() + "\\wwwroot\\json\\editions.json";
			Editions = JsonConvert.DeserializeObject<Edition[]>(System.IO.File.ReadAllText(pathToJson));
        }
    }
    public record Edition(string Name, string Description);
}
