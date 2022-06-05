using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using Markdig;
using Markdig.Extensions.Yaml;
using Markdig.Renderers;
using Markdig.Syntax;
using Microsoft.AspNetCore.Html;
using YamlDotNet.Serialization;

namespace OpenTabletDriver.Web.Models
{
    public class MarkdownViewModel
    {
        public MarkdownViewModel(string markdown)
        {
            var pipeline = new MarkdownPipelineBuilder()
                .UseYamlFrontMatter()
                .UseAdvancedExtensions()
                .Build();

            using (var sw = new StringWriter())
            {
                var renderer = new HtmlRenderer(sw);
                pipeline.Setup(renderer);

                var document = Markdown.Parse(markdown, pipeline);

                var yamlBlock = document.Descendants<YamlFrontMatterBlock>().FirstOrDefault();
                if (yamlBlock != null)
                {
                    var startIndex = yamlBlock.Span.Start + 4;
                    var length = yamlBlock.Span.Length - 8;
                    var yaml = markdown.Substring(startIndex, length);

                    Metadata = new Deserializer().Deserialize<Dictionary<string, object>>(yaml);
                }

                renderer.Render(document);
                Content = GetHtmlContent(sw);
            }
        }

        public IDictionary<string, object> Metadata { get; }
        public IHtmlContent Content { get; }

        private static readonly Regex HeaderRegex = new Regex("(?<=.?\n)<h3", RegexOptions.Compiled);

        private IHtmlContent GetHtmlContent(StringWriter sw)
        {
            sw.Flush();
            var html = sw.ToString()!;

            var formattedHtml = HeaderRegex.Replace(html, "<hr><h3")
                .Replace("<h3", "<h3 class=\"wiki-nav-item pb-2\"")
                .Replace("<table>", "<table class=\"table table-hover ms-3\">")
                .Replace("<p>", "<p class=\"ms-3\">");

            return new HtmlString(formattedHtml);
        }
    }
}