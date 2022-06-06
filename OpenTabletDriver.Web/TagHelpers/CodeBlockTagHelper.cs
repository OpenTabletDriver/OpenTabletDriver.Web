using System;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc.Rendering;
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

            var rawContent = (await output.GetChildContentAsync()).GetContent();
            var body = TrimBaseIndentation(rawContent);

            var code = new TagBuilder("code");
            code.AddCssClass($"language-{Language}");
            code.InnerHtml.SetHtmlContent($"{body}\n");

            output.Content.SetHtmlContent(code);
        }

        private string TrimBaseIndentation(string content)
        {
            var endsTrimmedContent = TrimStartRegex.Replace(content.TrimEnd(), "$1", 1);
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

                    return endsTrimmedContent;
                }

                lines[i] = line.Substring(Math.Min(baseIndentationLength, line.Length)).TrimEnd();
            }

            return String.Join(Environment.NewLine, lines);
        }

        private static Regex TrimStartRegex = new Regex("^\\s*\n(\\s*)(?=\\S*)", RegexOptions.Compiled);

        private int CountIndentation(string line)
        {
            for (var i = 0; i < line.Length; i++)
                if (!char.IsWhiteSpace(line[i]))
                    return i;
            return int.MaxValue;
        }
    }
}
