using Microsoft.AspNetCore.Razor.TagHelpers;

namespace Task_1.TagHelpers
{
    public class ContainerTagHelper : TagHelper
    {
        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            output.TagName = "div";
            output.TagMode = TagMode.StartTagAndEndTag;
            output.Attributes.SetAttribute("class", "container");
        }
    }
}
