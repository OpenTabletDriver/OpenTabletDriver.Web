using Markdig;
using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace OpenTabletDriver.Web
{
    public class Markdown
    {
        public static string ToHtml(string markdown)
        {
            var pipeline = new MarkdownPipelineBuilder()
                .UseAdvancedExtensions()
                .Build();
            return Markdig.Markdown.ToHtml(markdown, pipeline);
        }

        public static IHtmlContent Render(string markdown)
        {
            return new HtmlString(ToHtml(markdown));
        }
    }
}