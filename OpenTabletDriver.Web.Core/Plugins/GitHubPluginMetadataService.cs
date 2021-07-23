using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using ICSharpCode.SharpZipLib.GZip;
using ICSharpCode.SharpZipLib.Tar;
using Newtonsoft.Json;
using OpenTabletDriver.Web.Core.Services;

namespace OpenTabletDriver.Web.Core.Plugins
{
    public class GitHubPluginMetadataService : IPluginMetadataService
    {
        public const string REPOSITORY_OWNER = "OpenTabletDriver";
        public const string REPOSITORY_NAME = "Plugin-Repository";

        public async Task<IEnumerable<PluginMetadata>> GetPlugins()
        {
            var plugins = await DownloadAsync(REPOSITORY_OWNER, REPOSITORY_NAME);
            return plugins.OrderBy(p => p.Name).Distinct(new PluginMetadataComparer());
        }

        private static HttpClient GetClient()
        {
            var client = new HttpClient();
            client.DefaultRequestHeaders.Add("User-Agent", "OpenTabletDriver-Web");
            return client;
        }

        public static async Task<IEnumerable<PluginMetadata>> DownloadAsync(string owner, string name,
            string gitRef = null)
        {
            string archiveUrl = $"https://api.github.com/repos/{owner}/{name}/tarball/{gitRef}";
            return await DownloadAsync(archiveUrl);
        }

        public static async Task<IEnumerable<PluginMetadata>> DownloadAsync(string archiveUrl)
        {
            using (var client = GetClient())
            using (var httpStream = await client.GetStreamAsync(archiveUrl))
                return FromStream(httpStream);
        }

        public static IEnumerable<PluginMetadata> FromStream(Stream stream)
        {
            var memStream = new MemoryStream();
            stream.CopyTo(memStream);

            using (memStream)
            using (var gzipStream = new GZipInputStream(memStream))
            using (var archive = TarArchive.CreateInputTarArchive(gzipStream, null))
            {
                string hash = CalculateSHA256(memStream);
                string temp = Path.GetTempPath();

                Directory.CreateDirectory(temp);
                string cacheDir = Path.Join(temp, $"{hash}-OpenTabletDriver-PluginMetadata");

                if (!Directory.Exists(cacheDir))
                    archive.ExtractContents(cacheDir);

                return EnumeratePluginMetadata(cacheDir);
            }
        }

        protected static string CalculateSHA256(Stream stream)
        {
            using (var sha256 = SHA256Managed.Create())
            {
                var hashData = sha256.ComputeHash(stream);
                stream.Position = 0;
                var sb = new StringBuilder();
                foreach (var val in hashData)
                {
                    var hex = val.ToString("x2");
                    sb.Append(hex);
                }

                return sb.ToString();
            }
        }

        protected static IEnumerable<PluginMetadata> EnumeratePluginMetadata(string directoryPath)
        {
            var serializer = new JsonSerializer();
            foreach (var file in Directory.EnumerateFiles(directoryPath, "*.json", SearchOption.AllDirectories))
            {
                using (var fs = File.OpenRead(file))
                using (var sr = new StreamReader(fs))
                using (var jr = new JsonTextReader(sr))
                    yield return serializer.Deserialize<PluginMetadata>(jr);
            }
        }

        private class PluginMetadataComparer : IEqualityComparer<PluginMetadata>
        {
            public bool Equals(PluginMetadata x, PluginMetadata y)
            {
                return x.Name == y.Name &&
                    x.Owner == y.Owner &&
                    x.RepositoryUrl == y.RepositoryUrl;
            }

            public int GetHashCode(PluginMetadata obj)
            {
                return HashCode.Combine(obj.Name, obj.Owner, obj.RepositoryUrl);
            }
        }
    }
}