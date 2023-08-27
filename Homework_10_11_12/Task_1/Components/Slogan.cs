using Microsoft.AspNetCore.Mvc.ViewComponents;
using Microsoft.AspNetCore.Mvc;

namespace Task_1.Components
{
    [ViewComponent]
    public class Slogan
    {
        public IViewComponentResult Invoke() => new ContentViewComponentResult("Get ready to explore a wide range of products!");
    }
}
