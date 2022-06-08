using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using OpenTabletDriver.Web.Core.Services;

#nullable enable

namespace OpenTabletDriver.Web.Controllers
{
    public class ReleaseController : Controller
    {
        private readonly IReleaseService _releaseService;

        public ReleaseController(IReleaseService releaseService)
        {
            _releaseService = releaseService;
        }

        [Route("{Release}/{Download}/{file}")]
        public async Task<IActionResult> Download(string file)
        {
            var release = await _releaseService.GetLatestRelease();
            var assets = await release.GetReleaseAssets();
            var asset = assets.First(a => a.FileName == file);
            return Redirect(asset.Url);
        }
    }
}
