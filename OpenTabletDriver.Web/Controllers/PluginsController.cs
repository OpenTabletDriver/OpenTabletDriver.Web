using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using OpenTabletDriver.Web.Core.Services;

namespace OpenTabletDriver.Web.Controllers
{
    public class PluginsController : Controller
    {
        public PluginsController(IPluginMetadataService pluginMetadataService)
        {
            this.pluginMetadataService = pluginMetadataService;
        }

        private IPluginMetadataService pluginMetadataService;

        public async Task<IActionResult> Index()
        {
            var metadata = await pluginMetadataService.GetPlugins();
            return View(metadata);
        }
    }
}