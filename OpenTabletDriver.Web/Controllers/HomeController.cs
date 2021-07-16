using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using OpenTabletDriver.Web.Core.Contracts;
using OpenTabletDriver.Web.Core.Services;
using OpenTabletDriver.Web.Models;
using OpenTabletDriver.Web.Utilities;

namespace OpenTabletDriver.Web.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> logger;
        private readonly IReleaseService releaseService;

        public HomeController(ILogger<HomeController> logger, IReleaseService releaseService)
        {
            this.logger = logger;
            this.releaseService = releaseService;
        }

        private IEnumerable<IRelease> releases;

        public const string REPOSITORY_URL = "https://www.github.com/OpenTabletDriver/OpenTabletDriver";

        [ResponseCache(Duration = 300)]
        public async Task<IActionResult> Index()
        {
            return View(releases ??= await releaseService.GetAllReleases());
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            var viewModel = new ErrorViewModel
            {
                RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier
            };
            return View(viewModel);
        }
    }
}