using System;
using System.Collections.Generic;
using System.Formats.Asn1;
using System.Linq;
using System.Threading.Tasks;
using Octokit;
using OpenTabletDriver.Web.Core.Contracts;
using OpenTabletDriver.Web.Core.Services;

namespace OpenTabletDriver.Web.Core.GitHub.Services
{
    public class GitHubReleaseService : IReleaseService
    {
        public async Task<IEnumerable<IRelease>> GetAllReleases()
        {
            if (await GitHubCore.GetClient() is GitHubClient client)
            {
                var repo = await GitHubCore.GetRepository(client);
                var releases = await client.Repository.Release.GetAll(repo.Id);

                return releases.Select(r => new GitHubRelease(r));
            }

            return null;
        }

        public async Task<IRelease> GetLatestRelease()
        {
            if (await GitHubCore.GetClient() is GitHubClient client)
            {
                var repo = await GitHubCore.GetRepository(client);
                var release = await client.Repository.Release.GetLatest(repo.Id);

                return new GitHubRelease(release);
            }

            return null;
        }

        public async Task<IRelease> GetLatestWorkflowRun()
        {
            if (await GitHubCore.GetClient() is GitHubClient client)
            {
                var latestCommit = await GitHubCore.GetLatestCommit(client);
                var runsResponse = await client.Check.Run.GetAllForReference(latestCommit.Repository.Id, latestCommit.Ref);

                var targetRun = runsResponse.CheckRuns.FirstOrDefault(r => r.Name == GitHubCore.DOTNET_WORKFLOW_RUN);
                return new GitHubWorkflowRun(targetRun);
            }

            throw new NotImplementedException(); // TODO: implement
        }
    }
}