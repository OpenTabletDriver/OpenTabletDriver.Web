using System.IO;
using System.Net.Http;
using System.Runtime.Versioning;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc;
using OpenTabletDriver.Web.Core.Services;
using OpenTabletDriver.Web.Models;

namespace OpenTabletDriver.Web.Controllers
{
    public class TabletsController : Controller
    {
        private const string TABLETS_MARKDOWN_URL =
            "https://github.com/OpenTabletDriver/OpenTabletDriver/raw/master/TABLETS.md";

        [ResponseCache(Duration = 300)]
        public async Task<IActionResult> Index(string search = null)
        {
            using (var client = new HttpClient())
            using (var httpStream = await client.GetStreamAsync(TABLETS_MARKDOWN_URL))
            using (var sr = new StreamReader(httpStream))
            {
                string markdown = await sr.ReadToEndAsync();
                string html = Markdown.ToHtml(markdown);
                string patchedHtml = html.Replace(
                    "<table>",
                    "<table id=\"tablets\" class=\"table table-hover\">"
                );

                var model = new ContentSearchViewModel
                {
                    Content = new HtmlString(patchedHtml),
                    Search = search
                };
                return View(model);
            }
        }
    }
}