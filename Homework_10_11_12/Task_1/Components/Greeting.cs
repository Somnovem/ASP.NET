using Microsoft.AspNetCore.Mvc;

namespace Task_1.Components
{
    public class Greeting : ViewComponent
    {
        public IViewComponentResult Invoke() => Content("Welcome to Lorem Store!");
    }
}
