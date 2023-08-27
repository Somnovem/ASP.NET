using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ViewComponents;

namespace Task_1.Components
{
    [ViewComponent]
    public class Date
    {
        public IViewComponentResult Invoke()
        {
            return new HtmlContentViewComponentResult(
             new HtmlString("<div class=\"date-container\">" +
                "<div class=\"date\" id=\"currentDate\"></div>" +
                "<div class=\"weekday\"></div>" +
                "<div class=\"month\"></div>" +
             "</div>")
            );
        }
    }
}
