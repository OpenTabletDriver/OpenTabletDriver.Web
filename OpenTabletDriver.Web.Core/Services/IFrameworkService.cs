using System.Threading.Tasks;

namespace OpenTabletDriver.Web.Core.Services
{
    public interface IFrameworkService
    {
        string GetLatestVersionUrl(FrameworkPlatform platform, string archetecture);
    }
}