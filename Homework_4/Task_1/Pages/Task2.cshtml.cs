using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Task_1.Pages
{
    public class Task2Model : PageModel
    {
        public string RandomEnglishPhrase { get; set; }
        public void OnGet()
        {
            Random random = new Random();
            for (int i = 0; i < random.Next(5, 11); i++)
            {
                int letterValue = random.Next(65, 91);
                if (random.Next(0, 2) == 0) letterValue += 32;
                RandomEnglishPhrase += (char)letterValue;
            }
        }
    }
}
