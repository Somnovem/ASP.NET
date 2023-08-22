using Microsoft.AspNetCore.Mvc;
using System.IO;
using Newtonsoft.Json;
using Practice_10.Models;
using System.Text;

namespace Practice_10.Controllers
{
	public class HomeController : Controller
	{
        private readonly IWebHostEnvironment _webHostEnvironment;
        public HomeController(IWebHostEnvironment webHostEnvironment)
        {
            _webHostEnvironment = webHostEnvironment;
        }
        public IActionResult Index()
		{
			ViewData["Welcome Message"] = "Welcome to Ubuntu";
			ViewBag.WelcomeDescr = "Your gateway to a powerful and versatile operating system.";
            return View();
		}

		public IActionResult Developer()
		{
            string jsonFilePath = Path.Combine(_webHostEnvironment.WebRootPath, "json", "changes.json");
            string json = System.IO.File.ReadAllText(jsonFilePath);
            return View(JsonConvert.DeserializeObject<VersionViewModel>(json));
		}

        [HttpGet]
        public IActionResult Review() => View();

        [HttpPost]
        public string Review(string email, string? firstname, string review)
        {
            StringBuilder result = new StringBuilder();
            if (!string.IsNullOrEmpty(firstname))
            {
                result.Append($"Dear {firstname},");
            }
            result.Append($"Thanks for your review! Our moderators will look into your feedback, and respond to you at {email}");
            return result.ToString();
        }

    }
}