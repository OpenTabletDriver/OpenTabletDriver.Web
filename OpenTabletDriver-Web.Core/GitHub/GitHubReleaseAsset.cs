using System.IO;
using System.Net.Http;
using Octokit;
using OpenTabletDriver.Web.Core.Contracts;

namespace OpenTabletDriver.Web.Core.GitHub.Services
{
    public class GitHubReleaseAsset : IReleaseAsset
    {
        public GitHubReleaseAsset(ReleaseAsset asset)
        {
            this.asset = asset;
        }

        private ReleaseAsset asset;

        public string FileName => asset.Name;
        public string Url => asset.BrowserDownloadUrl;
    }
}