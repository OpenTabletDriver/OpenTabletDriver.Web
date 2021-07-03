using System;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Octokit;
using OpenTabletDriver_Web.Core.GitHub.API;
using OpenTabletDriver.Web.Core.Contracts;
using OpenTabletDriver.Web.Core.GitHub.Services;

namespace OpenTabletDriver.Web.Core.GitHub
{
    [JsonObject(MemberSerialization.OptIn)]
    public class GitHubWorkflowArtifact : IReleaseAsset
    {
        public GitHubWorkflowArtifact(GitHubAssetResponse assetResponse, CheckSuite suite)
        {
            this.assetResponse = assetResponse;
            this.suite = suite;
        }

        private GitHubAssetResponse assetResponse;
        private CheckSuite suite;

        public string FileName => assetResponse.Name + ".zip";

        public string Url =>
            string.Format("https://github.com/{0}/{1}/suites/{2}/artifacts/{3}", GitHubCore.REPOSITORY_OWNER,
                GitHubCore.REPOSITORY_NAME, suite.Id, assetResponse.Id);
    }
}