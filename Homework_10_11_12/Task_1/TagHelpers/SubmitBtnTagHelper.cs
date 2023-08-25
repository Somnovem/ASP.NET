using Microsoft.AspNetCore.Razor.TagHelpers;

namespace Task_1.TagHelpers
{
    public class SubmitBtnTagHelper : TagHelper
    {
        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            output.TagName = "button";
            output.TagMode = TagMode.StartTagAndEndTag;
            output.Attributes.SetAttribute("class", "btn");
            output.Attributes.SetAttribute("type", "submit");
        }
    }
}
