using Octokit;
using OpenTabletDriver.Web.Core.Contracts;

namespace OpenTabletDriver.Web.Core.GitHub
{
    public class GitHubReleaseAsset : IReleaseAsset
    {
        private readonly ReleaseAsset asset;

        public GitHubReleaseAsset(ReleaseAsset asset)
        {
            this.asset = asset;
        }

        public string FileName => asset.Name;
        public string Url => asset.BrowserDownloadUrl;
    }
}