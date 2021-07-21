using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages.Infrastructure;
using Octokit;
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

        public IActionResult Index()
        {
            string userAgentHeader = Request.Headers["User-Agent"];
            string userAgent = userAgentHeader.ToLower();
            FrameworkPlatform platform = FrameworkPlatform.Unknown;
            FrameworkArchetecture archetecture = FrameworkArchetecture.Unknown;

            if (userAgent.Contains("windows"))
                platform = FrameworkPlatform.Windows;
            else if (userAgent.Contains("linux"))
                platform = FrameworkPlatform.Linux;
            else if (userAgent.Contains("mac"))
                platform = FrameworkPlatform.MacOS;

            if (platform == FrameworkPlatform.MacOS || userAgent.Contains("x86_64") || userAgent.Contains("x64"))
                archetecture = FrameworkArchetecture.x64;
            else if (userAgent.Contains("x86"))
                archetecture = FrameworkArchetecture.x86;
            else if (userAgent.Contains("arm64"))
                archetecture = FrameworkArchetecture.ARM64;

            string url = frameworkService.GetLatestVersionUrl(platform, archetecture);
            return Redirect(url);
        }
    }
}