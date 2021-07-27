using Microsoft.AspNetCore.Mvc;
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
            FrameworkArchitecture architecture = FrameworkArchitecture.x64;

            if (userAgent.Contains("windows"))
                platform = FrameworkPlatform.Windows;
            else if (userAgent.Contains("linux"))
                platform = FrameworkPlatform.Linux;
            else if (userAgent.Contains("mac"))
                platform = FrameworkPlatform.MacOS;

            if (platform == FrameworkPlatform.MacOS || userAgent.Contains("x86_64") || userAgent.Contains("x64"))
                architecture = FrameworkArchitecture.x64;
            else if (userAgent.Contains("x86"))
                architecture = FrameworkArchitecture.x86;
            else if (userAgent.Contains("arm64"))
                architecture = FrameworkArchitecture.ARM64;

            string url = frameworkService.GetLatestVersionUrl(platform, architecture);
            return Redirect(url);
        }
    }
}