using Microsoft.AspNetCore.Razor.TagHelpers;

namespace MyAspNetCore.Web.TagHelpers
{
	public class ItalicTagHelper:TagHelper
	{
		public override void Process(TagHelperContext context, TagHelperOutput output)
		{
			output.PreContent.SetHtmlContent("<i>");
			output.PostContent.SetHtmlContent("</i>");
		}
	}
}
