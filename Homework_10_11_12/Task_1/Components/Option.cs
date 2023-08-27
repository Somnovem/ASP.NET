using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ViewComponents;

namespace Task_1.Components
{
    [ViewComponent]
    public class Option
    {
        public IViewComponentResult Invoke(string value, string content, bool disabled, bool selected)
        {
            return new HtmlContentViewComponentResult(
             new HtmlString($"<option value={value} {(disabled ? "disabled" : "")} {(selected ? "selected" : "")}>{content}</option>")
            );
        }
    }
}
