using Microsoft.AspNetCore.Mvc;
using Task_2.Models;
using Newtonsoft.Json;
using static Task_2.Models.EditionsViewModel;

namespace Task_2.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Editions()
        {
            var viewModel = new EditionsViewModel();
			string pathToJson = Directory.GetCurrentDirectory() + "\\wwwroot\\json\\editions.json";
			viewModel.Editions = JsonConvert.DeserializeObject<Edition[]>(System.IO.File.ReadAllText(pathToJson));
			return View(viewModel);
        }

        public IActionResult References()
        {
            var viewModel = new ReferencesViewModel();
            string pathToJson = Directory.GetCurrentDirectory() + "\\wwwroot\\json\\references.json";
			viewModel.DNDShows = JsonConvert.DeserializeObject<DNDShow[]>(System.IO.File.ReadAllText(pathToJson));
            return View(viewModel);
        }
    }
}
