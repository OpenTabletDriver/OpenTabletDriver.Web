using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using OpenTabletDriver.Web.Core.Services;
using OpenTabletDriver.Web.Models;

#nullable enable

namespace OpenTabletDriver.Web.Controllers
{
    public class WikiController : Controller
    {
        private readonly IMarkdownSource _markdownSource;
        private readonly IMemoryCache _memoryCache;

        public WikiController(IMarkdownSource markdownSource, IMemoryCache memoryCache)
        {
            _markdownSource = markdownSource;
            _memoryCache = memoryCache;
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
            var route = $"{category}/{page}";
            var model = await _memoryCache.GetOrCreateAsync(route, entry => GenerateMarkdownWikiPage(entry, category, page));

            return model == null ? View(route) : View("WikiMarkdownPage", model);
        }

        private async Task<MarkdownViewModel?> GenerateMarkdownWikiPage(ICacheEntry entry, string category, string page)
        {
            entry.AbsoluteExpiration = DateTimeOffset.MaxValue;
            var markdown = await _markdownSource.GetPage(category, page);
            return markdown != null ? new MarkdownViewModel(markdown) : null;
        }
    }
}
