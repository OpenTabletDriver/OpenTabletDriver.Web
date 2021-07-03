using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Octokit;
using OpenTabletDriver.Web.Core.Contracts;

namespace OpenTabletDriver.Web.Core.GitHub.Services
{
    public class GitHubRelease : IRelease
    {
        public GitHubRelease(Release release)
        {
            this.release = release;
        }

        private Release release;
        private IEnumerable<ReleaseAsset> releaseAssets;

        public string Name => release.TagName;
        public string Tag => release.TagName;
        public string Url => release.HtmlUrl;
        public string Body => release.Body;
        public DateTimeOffset Date => release.PublishedAt ?? release.CreatedAt;

        public async Task<IEnumerable<IReleaseAsset>> GetReleaseAssets()
        {
            if (await GitHubCore.GetClient() is GitHubClient client)
            {
                var repo = await GitHubCore.GetRepository();
                releaseAssets ??= await client.Repository.Release.GetAllAssets(repo.Id, release.Id);
                return releaseAssets.Select(r => new GitHubReleaseAsset(r));
            }

            return Array.Empty<IReleaseAsset>();
        }
    }
}