using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages.Infrastructure;
using OpenTabletDriver.Web.Core;
using OpenTabletDriver.Web.Core.Services;

namespace OpenTabletDriver.Web.Controllers
{
    public class FrameworkController : Controller
    {
        public FrameworkController(IFrameworkService frameworkService)
        {
            this.frameworkService = frameworkService;
        }

        private IFrameworkService frameworkService;

        public IActionResult Latest()
        {
            string userAgent = Request.Headers["User-Agent"];
            FrameworkPlatform platform = FrameworkPlatform.Unknown;
            string archetecture = null;

            if (userAgent.Contains("Windows"))
                platform = FrameworkPlatform.Windows;
            if (userAgent.Contains("Linux"))
                platform = FrameworkPlatform.Linux;
            if (userAgent.Contains("Mac"))
                platform = FrameworkPlatform.MacOS;

            if (platform == FrameworkPlatform.MacOS || userAgent.Contains("x86_64") || userAgent.Contains("x64"))
                archetecture = "x64";
            if (userAgent.Contains("x86"))
                archetecture = "x86";

            string url = frameworkService.GetLatestVersionUrl(platform, archetecture);
            return Redirect(url);
        }
    }
}