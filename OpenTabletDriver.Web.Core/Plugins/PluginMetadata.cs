using System;

namespace OpenTabletDriver.Web.Core.Plugins
{
    public class PluginMetadata
    {
        /// <summary>
        /// The name of the plugin.
        /// </summary>
        public string Name { set; get; }

        /// <summary>
        /// The owner of the plugin's source code repository.
        /// </summary>
        public string Owner { set; get; }

        /// <summary>
        /// The plugin's long description.
        /// </summary>
        public string Description { set; get; }

        /// <summary>
        /// The plugins' version.
        /// Newer supported versions will be preferred by default.
        /// </summary>
        public Version PluginVersion { set; get; }

        /// <summary>
        /// The plugin's minimum supported OpenTabletDriver version,
        /// </summary>
        public Version SupportedDriverVersion { set; get; }

        /// <summary>
        /// The plugin's source code repository URL.
        /// </summary>
        public string RepositoryUrl { set; get; }

        /// <summary>
        /// The plugin's binary download URL.
        /// </summary>
        public string DownloadUrl { set; get; }

        /// <summary>
        /// The compression format used in the binary download from <see cref="DownloadUrl"/>.
        /// </summary>
        public string CompressionFormat { set; get; }

        /// <summary>
        /// The SHA256 hash of the file at <see cref="DownloadUrl"/>, used for verifying file integrity.
        /// </summary>
        public string SHA256 { set; get; }

        /// <summary>
        /// The plugin's wiki URL.
        /// </summary>
        public string WikiUrl { set; get; }

        /// <summary>
        /// The SPDX license identifier expression.
        /// </summary>
        public string LicenseIdentifier { set; get; }

        public static bool Match(PluginMetadata primary, PluginMetadata secondary)
        {
            if (primary == null || secondary == null)
                return false;

            return primary.Name == secondary.Name &&
                primary.Owner == secondary.Owner &&
                primary.RepositoryUrl == secondary.RepositoryUrl;
        }
    }
}