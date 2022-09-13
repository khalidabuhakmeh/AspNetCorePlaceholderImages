using Microsoft.AspNetCore.Razor.TagHelpers;

namespace Bespoke;

[HtmlTargetElement("placeholder", TagStructure = TagStructure.WithoutEndTag)] 
public class PlaceholderTagHelper : TagHelper
{
    public int Width { get; set; } = 256;
    public int? Height { get; set; }
    
    public override void Process(TagHelperContext context, TagHelperOutput output)
    {
        output.TagName = "img";
        var src = $"/img/placeholder.png?width={Width}&height={Height ?? Width}&rmode=stretch&dim=true";

        output.Attributes.SetAttribute("src", src);
    }
}