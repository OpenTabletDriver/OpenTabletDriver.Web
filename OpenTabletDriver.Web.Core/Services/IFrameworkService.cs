namespace OpenTabletDriver.Web.Core.Services
{
    public interface IFrameworkService
    {
        string GetLatestVersionUrl(FrameworkPlatform platform, FrameworkArchitecture architecture);
    }
}
