using System;
using Newtonsoft.Json;

namespace OpenTabletDriver_Web.Core.GitHub.API
{
    [JsonObject]
    public class GitHubAssetResponse
    {
        [JsonProperty("id")]
        public long Id { set; get; }
        
        [JsonProperty("node_id")]
        public string NodeId { set; get; }
        
        [JsonProperty("name")]
        public string Name { set; get; }
        
        [JsonProperty("size_in_bytes")]
        public uint SizeInBytes { set; get; }
        
        [JsonProperty("url")]
        public string Url { set; get; }
        
        [JsonProperty("archive_download_url")]
        public string ArchiveDownloadUrl { set; get; }
        
        [JsonProperty("expired")]
        public bool Expired { set; get; }
        
        [JsonProperty("created_at")]
        public DateTime CreatedAt { set; get; }
        
        [JsonProperty("updated_at")]
        public DateTime UpdatedAt { set; get; }
        
        [JsonProperty("expires_at")]
        public DateTime ExpiresAt { set; get; }
    }
}