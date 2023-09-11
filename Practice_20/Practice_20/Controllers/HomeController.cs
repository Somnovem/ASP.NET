using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Practice_20.Models;
using Practice_20.ViewModels;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;

namespace Practice_20.Controllers
{
    public class HomeController : Controller
    {
        private readonly ApplicationContext _context;
        private readonly ILogger<HomeController> _logger;
        
        public HomeController(ApplicationContext db, ILogger<HomeController> logger)
        {
            _context = db;
            _logger = logger;
        }
        
        public IActionResult Index()
        {
            return View();
        }
        
        [Authorize(Policy = "AgeLimit")]
        public IActionResult Loans()
        {
            return View();
        }
        
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}