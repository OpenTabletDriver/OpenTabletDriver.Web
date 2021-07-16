using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Threading.Tasks;
using Markdig.Renderers.Html;
using Microsoft.AspNetCore.Components.Server;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.Extensions.Logging;
using OpenTabletDriver.Web.Core.Contracts;
using OpenTabletDriver.Web.Core.Services;

namespace OpenTabletDriver.Web.Controllers
{
    public class ChangelogController : Controller
    {
        public ChangelogController(IReleaseService releaseService)
        {
            this.releaseService = releaseService;
        }

        private IReleaseService releaseService;

        [ResponseCache(Duration = 300)]
        public async Task<IActionResult> Index()
        {
            var releases = await releaseService.GetAllReleases();
            return View(releases);
        }

        public async Task<IActionResult> GetChangelog([NotNull] string tag)
        {
            var release = await releaseService.GetRelease(tag);
            return PartialView("Release/_Changelog", release);
        }
    }
}