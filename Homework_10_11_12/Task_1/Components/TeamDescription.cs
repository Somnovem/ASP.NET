using Microsoft.AspNetCore.Mvc.ViewComponents;
using Microsoft.AspNetCore.Mvc;

namespace Task_1.Components
{
    [ViewComponent]
    public class TeamDescription
    {
        public IViewComponentResult Invoke() => new ContentViewComponentResult("Our team of tech enthusiasts is passionate about staying up-to-date with the latest trends and innovations in the tech world. Whether you're a casual user or a tech-savvy professional, we have something for everyone.");
    }
}
