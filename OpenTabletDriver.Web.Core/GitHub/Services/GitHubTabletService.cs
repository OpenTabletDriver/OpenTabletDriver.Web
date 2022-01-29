using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.Extensions.Caching.Memory;
using OpenTabletDriver.Web.Core.Services;

namespace OpenTabletDriver.Web.Core.GitHub.Services
{
    public class GitHubTabletService : ITabletService
    {
        private readonly IRepositoryService repositoryService;
        private readonly IMemoryCache cache;

        public GitHubTabletService(
            IRepositoryService repositoryService,
            IMemoryCache cache
        )
        {
            this.repositoryService = repositoryService;
            this.cache = cache;
        }

        private static readonly TimeSpan Expiration = TimeSpan.FromMinutes(5);

        public Task<string> GetMarkdownRaw()
        {
            const string key = nameof(GitHubTabletService) + nameof(GetMarkdownRaw);
            return cache.GetOrCreateAsync(key, GetMarkdownRawInternal);
        }

        public async Task<string> GetMarkdownRawInternal(ICacheEntry entry)
        {
            entry.AbsoluteExpirationRelativeToNow = Expiration;

            var repo = await repositoryService.GetRepository();

            using (var stream = await repositoryService.GetFileFromRepository(repo, "TABLETS.md"))
            using (var sr = new StreamReader(stream))
            {
                return await sr.ReadToEndAsync();
            }
        }
    }
}