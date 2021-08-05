using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.IO.Pipes;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Razor.TagHelpers;
using Microsoft.CodeAnalysis;

namespace OpenTabletDriver.Web.TagHelpers
{
    [HtmlTargetElement("codeblock")]
    public class CodeBlockTagHelper : TagHelper
    {
        public string Language { set; get; }
        
        public override async Task ProcessAsync(TagHelperContext context, TagHelperOutput output)
        {
            output.TagName = "pre";

            if (output.Attributes.TryGetAttribute("class", out var classAttr))
                output.Attributes.SetAttribute("class", $"{classAttr.Value} card card-body");
            else
                output.Attributes.Add("class", $"card card-body");


            var content = await output.GetChildContentAsync();
            var innerHtml = content.GetContent().Trim('\n');

            var body = TrimPreceding(innerHtml, ' ');
            output.Content.SetHtmlContent(body);
        }

        private string TrimPreceding(string value, char character)
        {
            var lines = value.Split(Environment.NewLine);
            int preceding = CountPreceding(lines, character);
            var trimmedLines = from line in lines
                select TrimPrecedingLine(line, character, preceding);

            var formattedLines = LanguageFormat(trimmedLines.ToArray(), Language);

            return string.Join(Environment.NewLine, formattedLines);
        }

        private IEnumerable<string> LanguageFormat(IList<string> lines, string language)
        {
            switch (language)
            {
                case "sh":
                case "bash":
                case "nix":
                {
                    for (int i = 0; i < lines.Count; i++)
                    {
                        var line = lines[i];
                        if (line.TrimStart().StartsWith("#"))
                        {
                            var nextLine = lines[i + 1];
                            const string tag = "span";
                            yield return $"<{tag} class=\"text-muted\">{line}</{tag}>{nextLine}";
                            i++;
                        }
                        else
                        {
                            yield return line;
                        }
                    }

                    break;
                }
                case "ini":
                {
                    for (int i = 0; i < lines.Count; i++)
                    {
                        var line = lines[i];
                        if (Regex.IsMatch(line, @"^\[.+?\]$"))
                        {
                            var nextLine = lines[i + 1];
                            const string tag = "span";
                            yield return $"<{tag} class=\"text-info\">{line}</{tag}>{nextLine}";
                            i++;
                        }
                        else
                        {
                            yield return line;
                        }
                    }

                    break;
                }
                default:
                {
                    foreach (var line in lines)
                        yield return line;
                    break;
                }
            }
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