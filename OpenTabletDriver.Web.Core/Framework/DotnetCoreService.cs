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
        private const string AKA_MS = @"https://aka.ms/dotnet";
        private const string CHANNEL = "current";

        public string GetLatestVersionUrl(FrameworkPlatform platform, FrameworkArchetecture archetecture)
        {
            string product = GetProduct(platform);
            string extension = GetExtension(platform);
            string os = GetNormalizedOS(platform);
            string arch = GetNormalizedArchitecture(archetecture);

            switch (platform)
            {
                case FrameworkPlatform.Linux:
                    // Link to package manager instructions instead of binaries or an installer.
                    return "https://docs.microsoft.com/en-us/dotnet/core/install/linux";
                case FrameworkPlatform.Windows:
                case FrameworkPlatform.MacOS:
                    return $"{AKA_MS}/{CHANNEL}/{product}-{os}-{arch}.{extension}";
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
                    return "tar.gz";
                case FrameworkPlatform.MacOS:
                    return "pkg";
                case FrameworkPlatform.Windows:
                    return "exe";
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

        private string GetNormalizedArchitecture(FrameworkArchetecture archetecture)
        {
            return archetecture switch
            {
                FrameworkArchetecture.x64 => "x64",
                FrameworkArchetecture.x86 => "x86",
                FrameworkArchetecture.ARM64 => "arm64",
                _ => null
            };
        }
    }
}