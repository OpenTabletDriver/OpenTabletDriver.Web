using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using OpenTabletDriver.Web.Core.Plugins;
using OpenTabletDriver.Web.Core.Services;
using OpenTabletDriver.Web.Models;

namespace OpenTabletDriver.Web.Controllers
{
    public class PluginsController : Controller
    {
        public PluginsController(IPluginMetadataService pluginMetadataService)
        {
            this.pluginMetadataService = pluginMetadataService;
        }

        private IPluginMetadataService pluginMetadataService;

        [ResponseCache(Duration = 300)]
        public async Task<IActionResult> Index(string search = null)
        {
            var model = new EnumerableSearchViewModel<PluginMetadata>()
            {
                Items = await pluginMetadataService.GetPlugins(),
                Search = search
            };
            return View(model);
        }
    }
}