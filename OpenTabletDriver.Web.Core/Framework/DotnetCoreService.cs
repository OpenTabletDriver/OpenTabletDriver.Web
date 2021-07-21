using System;
using System.Data.SqlTypes;
using System.Diagnostics;
using System.IO;
using System.Net.Http;
using System.Threading.Tasks;
using OpenTabletDriver.Web.Core.Services;

namespace OpenTabletDriver.Web.Core.Framework
{
    public class DotnetCoreService : IFrameworkService
    {
        private const string FEED = @"https://dotnetcli.azureedge.net/dotnet";
        private const string AKA_MS = @"https://aka.ms/dotnet";
        private const string CHANNEL = "current";
        private const string VERSION = "LATEST";

        public string GetLatestVersionUrl(FrameworkPlatform platform, string archetecture)
        {
            return ConstructDownloadUrl(platform, archetecture);
        }

        private string ConstructDownloadUrl(FrameworkPlatform platform, string archetecture)
        {
            string product = GetProduct(platform);
            string extension = GetExtension(platform);
            string os = GetNormalizedOS(platform);

            switch (platform)
            {
                case FrameworkPlatform.Linux:
                    return "https://docs.microsoft.com/en-us/dotnet/core/install/linux";
                case FrameworkPlatform.Windows:
                case FrameworkPlatform.MacOS:
                    return $"{AKA_MS}/{CHANNEL}/{product}-{os}-{archetecture}.{extension}";
                default:
                    throw new Exception("Unsupported platform.");
            }
        }

        private string GetProduct(FrameworkPlatform platform)
        {
            switch (platform)
            {
                case FrameworkPlatform.Linux:
                case FrameworkPlatform.MacOS:
                    return "dotnet-runtime";
                case FrameworkPlatform.Windows:
                    return "windowsdesktop-runtime";
                default:
                    return null;
            }
        }

        private string GetExtension(FrameworkPlatform platform)
        {
            switch (platform)
            {
                case FrameworkPlatform.Linux:
                case FrameworkPlatform.MacOS:
                    return "tar.gz";
                case FrameworkPlatform.Windows:
                    return "zip";
                default:
                    return null;
            }
        }

        private static string GetNormalizedOS(FrameworkPlatform platform)
        {
            return platform switch
            {
                FrameworkPlatform.Windows => "win",
                FrameworkPlatform.Linux => "linux",
                FrameworkPlatform.MacOS => "osx",
                _ => null
            };
        }
    }
}