using Microsoft.AspNetCore.Razor.TagHelpers;

namespace Task_1.TagHelpers
{
    public class ReviewTagHelper : TagHelper
    {
        public int UserId { get; set; }
        public string Message { get; set; }
        public DateTime Date { get; set; }
        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            output.TagName = "div";
            output.TagMode = TagMode.StartTagAndEndTag;
            output.Attributes.SetAttribute("style", "width: 100%; border:2px solid black; border-radius:15px; margin:15px 5px;");
            output.Attributes.SetAttribute("class", "card mb-4");
            output.Content.SetHtmlContent(
                "<div class=\"card-body\">" +
                    $"<p class=\"card-text\">User Id:{UserId}</p>" +
                    $"<p class=\"card-text\">Message: {Message}</p>" +
                    $"<p class=\"card-text\">Date: {Date.ToShortDateString()}" +
                "</div>"
            );
        }
    }
}
