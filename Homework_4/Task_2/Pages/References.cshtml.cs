using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Newtonsoft.Json;
namespace Task_2.Pages
{
    public class ReferencesModel : PageModel
    {
		[BindProperty]
		public DNDShow[] DNDShows { get; set; } = null;
		public void OnGet()
        {
			string pathToJson = Directory.GetCurrentDirectory() + "\\wwwroot\\json\\references.json";
			DNDShows = JsonConvert.DeserializeObject<DNDShow[]>(System.IO.File.ReadAllText(pathToJson));
		}
    }
    public record DNDShow(string Name, string Description, string WikiLink);
}
