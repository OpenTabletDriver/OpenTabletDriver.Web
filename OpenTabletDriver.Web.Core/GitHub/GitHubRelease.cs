using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Octokit;
using OpenTabletDriver.Web.Core.Contracts;
using OpenTabletDriver.Web.Core.GitHub.Services;
using OpenTabletDriver.Web.Core.Services;

namespace OpenTabletDriver.Web.Core.GitHub
{
    public class GitHubRelease : IRelease
    {
        private readonly IServiceProvider serviceProvider;
        private readonly IGitHubClient client;
        private readonly IRepositoryService repositoryService;
        private readonly Release release;

        public GitHubRelease(
            IServiceProvider serviceProvider,
            IGitHubClient client,
            IRepositoryService repositoryService,
            Release release
        )
        {
            this.serviceProvider = serviceProvider;
            this.client = client;
            this.repositoryService = repositoryService;
            this.release = release;
        }

        private IEnumerable<ReleaseAsset> releaseAssets;

        public string Name => release.TagName;
        public string Tag => release.TagName;
        public string Url => release.HtmlUrl;
        public string Body => release.Body;
        public DateTimeOffset Date => release.PublishedAt ?? release.CreatedAt;

        public async Task<IEnumerable<IReleaseAsset>> GetReleaseAssets()
        {
            var repo = await repositoryService.GetRepository();
            releaseAssets ??= await client.Repository.Release.GetAllAssets(repo.Id, release.Id);
            return releaseAssets.Select(r => serviceProvider.CreateInstance<GitHubReleaseAsset>(r));
        }
    }
}