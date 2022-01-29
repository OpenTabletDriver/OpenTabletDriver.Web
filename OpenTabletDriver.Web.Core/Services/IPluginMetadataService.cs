using System.Collections.Generic;
using System.Threading.Tasks;
using OpenTabletDriver.Web.Core.Plugins;

namespace OpenTabletDriver.Web.Core.Services
{
    public interface IPluginMetadataService
    {
        Task<IReadOnlyList<PluginMetadata>> GetPlugins();
    }
}