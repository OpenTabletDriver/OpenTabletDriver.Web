using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Markdig.Renderers.Html;
using Microsoft.AspNetCore.Components.Server;
using Microsoft.AspNetCore.Mvc;
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
        private IEnumerable<IRelease> releases;

        [ResponseCache(Duration = 300)]
        public async Task<IActionResult> Index()
        {
            return View(releases ??= await releaseService.GetAllReleases());
        }
    }
}