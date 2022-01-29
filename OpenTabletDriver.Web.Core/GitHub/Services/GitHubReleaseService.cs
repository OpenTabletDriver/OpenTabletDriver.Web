using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Caching.Memory;
using Octokit;
using OpenTabletDriver.Web.Core.Contracts;
using OpenTabletDriver.Web.Core.Services;

namespace OpenTabletDriver.Web.Core.GitHub.Services
{
    public class GitHubReleaseService : IReleaseService
    {
        private IServiceProvider serviceProvider;
        private IRepositoryService repositoryService;
        private IGitHubClient client;
        private IMemoryCache cache;

        public GitHubReleaseService(
            IServiceProvider serviceProvider,
            IRepositoryService repositoryService,
            IGitHubClient client,
            IMemoryCache cache
        )
        {
            this.serviceProvider = serviceProvider;
            this.repositoryService = repositoryService;
            this.client = client;
            this.cache = cache;
        }

        private static readonly TimeSpan Expiration = TimeSpan.FromMinutes(1);

        public Task<IReadOnlyList<IRelease>> GetAllReleases()
        {
            const string key = nameof(GitHubReleaseService) + nameof(GetAllReleases);
            return cache.GetOrCreateAsync(key, GetAllReleasesInternal);
        }

        public Task<IRelease> GetLatestRelease()
        {
            const string key = nameof(GitHubReleaseService) + nameof(GetLatestRelease);
            return cache.GetOrCreateAsync(key, GetLatestReleaseInternal);
        }

        public async Task<IRelease> GetRelease(string tag)
        {
            var repo = await repositoryService.GetRepository();
            var release = await client.Repository.Release.Get(repo.Id, tag);

            return serviceProvider.CreateInstance<GitHubRelease>(release);
        }

        private async Task<IRelease> GetLatestReleaseInternal(ICacheEntry entry)
        {
            entry.AbsoluteExpirationRelativeToNow = Expiration;

            var repo = await repositoryService.GetRepository();
            var release = await client.Repository.Release.GetLatest(repo.Id);

            return serviceProvider.CreateInstance<GitHubRelease>(release);
        }

        private async Task<IReadOnlyList<IRelease>> GetAllReleasesInternal(ICacheEntry entry)
        {
            entry.AbsoluteExpirationRelativeToNow = Expiration;

            var repo = await repositoryService.GetRepository();
            var releases = await client.Repository.Release.GetAll(repo.Id);
            return releases.Select(r => serviceProvider.CreateInstance<GitHubRelease>(r)).ToArray();
        }
    }
}