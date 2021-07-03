using Markdig;

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
    }
}