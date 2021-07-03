using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Threading.Tasks;

namespace OpenTabletDriver_Web.Core.GitHub.API
{
    public class GitHubAssetRestClient
    {
        private const string BASE_ADDRESS = "https://api.github.com/";
        private const string LIST_WORKFLOW_RUN_ARTIFACTS = "/repos/{0}/{1}/actions/runs/{2}/artifacts";

        public async Task<GitHubAssetGetResponse> Get(string owner, string name, long checkId)
        {
            var get = string.Format(LIST_WORKFLOW_RUN_ARTIFACTS, owner, name, checkId.ToString());

            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(get);
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                var response = await client.GetAsync(get);
                if (response.IsSuccessStatusCode)
                    return await response.Content.ReadFromJsonAsync<GitHubAssetGetResponse>();

                throw new HttpRequestException();
            }
        }
    }
}