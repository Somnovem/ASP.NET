using Microsoft.AspNetCore.Razor.TagHelpers;

namespace Task_1.TagHelpers
{
    public class MainTextTagHelper : TagHelper
    {
        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            output.TagName = "h1";
            output.TagMode = TagMode.StartTagAndEndTag;
            output.Attributes.SetAttribute("class", "display-4");
        }
    }
}
