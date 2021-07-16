using Newtonsoft.Json;

namespace OpenTabletDriver_Web.Core.GitHub.API
{
    [JsonObject]
    public class GitHubAssetGetResponse
    {
        [JsonProperty("total_count")]
        public int TotalCount { set; get; }

        [JsonProperty("artifacts")]
        public GitHubAssetResponse[] Artifacts { set; get; }
    }
}