using System.Reflection.Metadata;
using Markdig;
using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace OpenTabletDriver.Web
{
    public class Markdown
    {
        public static string ToHtml(string markdown)
        {
            if (string.IsNullOrWhiteSpace(markdown))
                return string.Empty;

            var pipeline = new MarkdownPipelineBuilder()
                .UseAdvancedExtensions()
                .Build();
            return Markdig.Markdown.ToHtml(markdown, pipeline);
        }

        public static IHtmlContent Render(string markdown)
        {
            if (string.IsNullOrWhiteSpace(markdown))
                return HtmlString.Empty;

            return new HtmlString(ToHtml(markdown));
        }
    }
}