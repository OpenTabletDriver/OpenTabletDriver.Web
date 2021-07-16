using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Octokit;
using OpenTabletDriver_Web.Core.GitHub.API;
using OpenTabletDriver.Web.Core.Contracts;
using OpenTabletDriver.Web.Core.GitHub.Services;

namespace OpenTabletDriver.Web.Core.GitHub
{
    public class GitHubWorkflowRun : IRelease
    {
        public GitHubWorkflowRun(CheckRun run)
        {
            this.run = run;
        }

        private CheckRun run;

        public string Name => run.Name;
        public string Tag => run.HeadSha;
        public string Url => run.HtmlUrl;
        public string Body { get; } = string.Empty;

        public DateTimeOffset Date => run.CompletedAt ?? run.StartedAt;

        public async Task<IEnumerable<IReleaseAsset>> GetReleaseAssets()
        {
            if (await GitHubCore.GetClient() is GitHubClient client)
            {
                var repo = await GitHubCore.GetRepository(client);
                var suites = await client.Check.Suite.GetAllForReference(repo.Id, run.HeadSha);
                var assetRestClient = new GitHubAssetRestClient();
                var response =
                    await assetRestClient.Get(GitHubCore.REPOSITORY_OWNER, GitHubCore.REPOSITORY_NAME, run.Id);
                return response.Artifacts.Select(
                    s => new GitHubWorkflowArtifact(s,
                        suites.CheckSuites.FirstOrDefault())); // todo: determine the correct suite
            }

            return null;
        }
    }
}