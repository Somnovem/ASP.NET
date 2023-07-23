using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Task_1.Pages
{
    public class IndexModel : PageModel
    {
        public int CurrentDayOfYear { get; set; }
        public bool IsLeapYear { get; set; } = false;
        
        private readonly ILogger<IndexModel> _logger;

        public IndexModel(ILogger<IndexModel> logger)
        {
            _logger = logger;
        }

        public void OnGet()
        {
            CurrentDayOfYear = DateTime.Now.DayOfYear;
            if (DateTime.Now.Year % 4 == 0) IsLeapYear = true;
            if(IsLeapYear && DateTime.Now.Year % 100 == 0) IsLeapYear = false;
            if(!IsLeapYear && DateTime.Now.Year % 400 == 0) IsLeapYear = true;
        }
    }
}