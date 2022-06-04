using System;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Razor.TagHelpers;

namespace OpenTabletDriver.Web.TagHelpers
{
    [HtmlTargetElement("codeblock")]
    public class CodeBlockTagHelper : TagHelper
    {
        public string Language { set; get; } = "plaintext";

        public override async Task ProcessAsync(TagHelperContext context, TagHelperOutput output)
        {
            output.TagName = "pre";
            var defaultClasses = "card";

            if (output.Attributes.TryGetAttribute("class", out var classAttr))
                output.Attributes.SetAttribute("class", $"{classAttr.Value} {defaultClasses}");
            else
                output.Attributes.Add("class", defaultClasses);

            var rawContent = (await output.GetChildContentAsync()).GetContent();
            var innerHTML = $"<code class='language-{Language}'>{TrimBaseIndentation(rawContent)}\n</code>";
            output.Content.SetHtmlContent(innerHTML);
        }

        private string TrimBaseIndentation(string content)
        {
            var endsTrimmedContent = content.TrimStart('\n').TrimEnd();
            var lines = endsTrimmedContent.Split(Environment.NewLine);
            var baseIndentationLength = CountIndentation(lines[0]);

            for (int i = 0; i != lines.Length; i++)
            {
                var line = lines[i];

                var indentationLength = CountIndentation(line);
                if (indentationLength < baseIndentationLength)
                {
                    if (indentationLength == line.Length)
                    {
                        lines[i] = "";
                        continue;
                    }

                    return new Regex("^\\s*\n(\\s*)(?=\\S*)").Replace(endsTrimmedContent, "$1", 1);
                }

                lines[i] = line.Substring(Math.Min(baseIndentationLength, line.Length));
            }

            return String.Join(Environment.NewLine, lines).Trim();
        }

        private int CountIndentation(string line) => line.TakeWhile(Char.IsWhiteSpace).Count();
    }
}
