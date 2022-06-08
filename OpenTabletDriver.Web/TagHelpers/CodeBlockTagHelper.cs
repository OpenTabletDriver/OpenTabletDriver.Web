using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Razor.TagHelpers;

namespace OpenTabletDriver.Web.TagHelpers
{
    [HtmlTargetElement("codeblock")]
    public class CodeBlockTagHelper : TagHelper
    {
        public string Language { set; get; }

        public override async Task ProcessAsync(TagHelperContext context, TagHelperOutput output)
        {
            output.TagName = "pre";

            var content = await output.GetChildContentAsync();
            var innerHtml = content.GetContent().Trim('\n');
            var body = TrimPreceding(innerHtml, ' ');

            var code = new TagBuilder("code");
            code.AddCssClass("hljs");
            code.AddCssClass($"language-{Language}");
            code.InnerHtml.SetHtmlContent(body);

            output.Content.SetHtmlContent(code);
        }

        private static string TrimPreceding(string value, char character)
        {
            var lines = value.Split(Environment.NewLine);
            var preceding = CountPreceding(lines, character);
            for (var i = 0; i < lines.Length; ++i)
            {
                var line = lines[i];
                lines[i] = line.Length >= preceding ? line[preceding..] : string.Empty;
            }

            return string.Join(Environment.NewLine, lines);
        }

        private static int CountPreceding(string[] lines, char leadingCharacter)
        {
            var min = int.MaxValue;
            foreach (var line in lines)
            {
                var lineSpan = line.AsSpan();
                for (var i = 0; i < lineSpan.Length; ++i)
                {
                    if (lineSpan[i] != leadingCharacter)
                    {
                        min = i < min ? i : min;
                        break;
                    }
                }
            }

            return min;
        }
    }
}
