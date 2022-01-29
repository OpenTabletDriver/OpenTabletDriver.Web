using System;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using ICSharpCode.SharpZipLib.GZip;
using ICSharpCode.SharpZipLib.Tar;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;
using Octokit;
using OpenTabletDriver.Web.Core.Services;

namespace OpenTabletDriver.Web.Core.GitHub.Services
{
    public class GitHubRepositoryService : IRepositoryService
    {
        private readonly ILogger<GitHubRepositoryService> logger;
        private readonly IGitHubClient client;
        private readonly HttpClient httpClient;
        private readonly IMemoryCache cache;

        public GitHubRepositoryService(
            ILogger<GitHubRepositoryService> logger,
            IGitHubClient client,
            HttpClient httpClient,
            IMemoryCache cache
        )
        {
            this.logger = logger;
            this.client = client;
            this.httpClient = httpClient;
            this.cache = cache;
        }

        private const string REPOSITORY_OWNER = "OpenTabletDriver";
        private const string REPOSITORY_NAME = "OpenTabletDriver";

        private static readonly TimeSpan Expiration = TimeSpan.FromMinutes(1);

        public Task<Repository> GetRepository()
        {
            const string key = nameof(GitHubRepositoryService) + nameof(GetRepository);
            return cache.GetOrCreateAsync(key, GetRepositoryInternal);
        }

        public async Task<string> DownloadRepositoryTarball(Repository repository)
        {
            var owner = repository.Owner.Login;
            var name = repository.Name;

            var tarballUrl = $"https://api.github.com/repos/{owner}/{name}/tarball/";
            return await DownloadAndExtract(tarballUrl, owner, name);
        }

        public async Task<Stream> GetFileFromRepository(Repository repository, string relativePath)
        {
            var archivePath = await DownloadRepositoryTarball(repository);
            var absolutePath = Path.Join(archivePath, relativePath);
            return File.OpenRead(absolutePath);
        }

        private async Task<string> DownloadAndExtract(string url, string owner, string name)
        {
            using (var httpStream = await httpClient.GetStreamAsync(url))
            using (var gzipStream = new GZipInputStream(httpStream))
            using (var archive = TarArchive.CreateInputTarArchive(gzipStream, Encoding.Default))
            {
                logger.LogInformation("Extracting tar archive for repository: {Owner}/{Name}", owner, name);

                var rootPath = Path.Join(Path.GetTempPath(), nameof(GitHubRepositoryService));
                var extractRootPath = Path.GetTempFileName();
                var dirPath = Path.Join(rootPath, owner, name);

                logger.LogInformation("Temporary extraction path: {ExtractRootPath}", extractRootPath);
                logger.LogInformation("Target extract path: {DirPath}", dirPath);

                if (Directory.Exists(dirPath))
                    Directory.Delete(dirPath, true);

                File.Delete(extractRootPath);
                Directory.CreateDirectory(extractRootPath);

                archive.ExtractContents(extractRootPath);

                var extractedDirPath = Directory.GetDirectories(extractRootPath).Single();
                Directory.CreateDirectory(Path.Join(rootPath, owner));
                Directory.Move(extractedDirPath, dirPath);

                if (Directory.Exists(extractRootPath))
                    Directory.Delete(extractRootPath);

                logger.LogInformation("Extracted {Owner}/{Name} successfully", owner, name);

                return dirPath;
            }
        }

        private Task<Repository> GetRepositoryInternal(ICacheEntry entry)
        {
            entry.AbsoluteExpirationRelativeToNow = Expiration;
            return client.Repository.Get(REPOSITORY_OWNER, REPOSITORY_NAME);
        }
    }
}