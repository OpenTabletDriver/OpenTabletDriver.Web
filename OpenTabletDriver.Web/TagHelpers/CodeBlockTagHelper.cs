using System;
using System.Collections.Generic;
using System.Linq;
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

            var content = await output.GetChildContentAsync();
            var innerHtml = content.GetContent().Trim('\n');
            var body = TrimPreceding(innerHtml, ' ');

            var code = new TagBuilder("code");
            code.AddCssClass($"language-{Language}");
            code.InnerHtml.SetHtmlContent(body);

            output.Content.SetHtmlContent(code);
        }

        private string TrimPreceding(string value, char character)
        {
            var lines = value.Split(Environment.NewLine);
            var preceding = CountPreceding(lines, character);
            var trimmedLines = from line in lines
                               select TrimPrecedingLine(line, character, preceding);

            return string.Join(Environment.NewLine, trimmedLines);
        }

        private int CountPreceding(IEnumerable<string> lines, char leadingCharacter)
        {
            foreach (var line in lines)
            {
                // Make sure that the line actually starts with the leading character
                if (line.StartsWith(leadingCharacter) == false)
                    continue;

                // Determine last index of leading character, return if something else is found
                for (var i = 0; i < line.Length; i++)
                {
                    var character = line[i];
                    if (character != leadingCharacter)
                        return i;
                }

                // Assume that this line is the template for trimming
                return line.Length;
            }

            throw new ArgumentException("No lines match the target leading character.", nameof(lines));
        }

        private string TrimPrecedingLine(string line, char character, int amount)
        {
            return line.StartsWith(character) ? new string(line.Skip(amount).ToArray()) : line;
        }
    }
}
