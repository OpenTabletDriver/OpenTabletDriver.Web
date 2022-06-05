using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc;
using OpenTabletDriver.Web.Core.Services;

#nullable enable

namespace OpenTabletDriver.Web.Controllers
{
    public class WikiController : Controller
    {
        private readonly IMarkdownSource _markdownSource;

        public WikiController(IMarkdownSource markdownSource)
        {
            _markdownSource = markdownSource;
        }

        [ResponseCache(Duration = 300)]
        public IActionResult Index()
        {
            return View();
        }

        [ResponseCache(Duration = 300)]
        [Route("/Wiki/{category}/{page}")]
        public async Task<IActionResult> Wiki(string category, string page)
        {
            var markdown = await _markdownSource.GetPage(category, page);
            return markdown == null ? View($"{category}/{page}") : View("WikiMarkdownPage", Format(markdown));
        }

        private static readonly Regex HeaderRegex = new Regex("(?<=.?\n)<h3", RegexOptions.Compiled);

        private IHtmlContent Format(string markdown)
        {
            var html = Markdown.ToHtml(markdown);

            var formatted = HeaderRegex.Replace(html, "<hr><h3")
                .Replace("<h3", "<h3 class=\"wiki-nav-item pb-2\"")
                .Replace("<table>", "<table class=\"table table-hover ms-3\">")
                .Replace("<p>", "<p class=\"ms-3\">");

            return new HtmlString(formatted);
        }
    }
}