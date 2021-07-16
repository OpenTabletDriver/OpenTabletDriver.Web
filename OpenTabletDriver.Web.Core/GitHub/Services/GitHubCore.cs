using System;
using System.IO;
using System.Threading.Tasks;
using Octokit;

namespace OpenTabletDriver.Web.Core.GitHub.Services
{
    internal static class GitHubCore
    {
        static GitHubCore()
        {
            Client = new GitHubClient(ProductHeaderValue.Parse(PRODUCT_HEADER))
            {
                Credentials = GetCredentials()
            };
        }

        public const string PRODUCT_HEADER = "OpenTabletDriver-Web";
        public const string REPOSITORY_OWNER = "OpenTabletDriver";
        public const string REPOSITORY_NAME = "OpenTabletDriver";
        public const string DOTNET_WORKFLOW_RUN = ".NET Core";

        public const int RATE_LIMIT_MINIMUM = 10;

        private static GitHubClient Client { get; }

        private static Credentials GetCredentials()
        {
            var apiKey = Environment.GetEnvironmentVariable("GITHUB_API");
            return string.IsNullOrWhiteSpace(apiKey) ? Credentials.Anonymous : new Credentials(apiKey);
        }

        public static async Task<GitHubClient> GetClient()
        {
            var rateLimits = await Client.Miscellaneous.GetRateLimits();
            return rateLimits.Resources.Core.Remaining > RATE_LIMIT_MINIMUM ? Client : null;
        }

        public static async Task<Repository> GetRepository(GitHubClient client = null)
        {
            if ((client ??= await GetClient()) is GitHubClient)
            {
                return await client.Repository.Get(REPOSITORY_OWNER, REPOSITORY_NAME);
            }

            return null;
        }

        public static async Task<GitReference> GetLatestCommit(GitHubClient client = null)
        {
            if ((client ??= await GetClient()) is GitHubClient)
            {
                var repository = await client.Repository.Get(REPOSITORY_OWNER, REPOSITORY_NAME);
                var masterBranch = await client.Repository.Branch.Get(repository.Id, "master");
                return masterBranch.Commit;
            }

            return null;
        }
    }
}