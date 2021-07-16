using System.IO;

namespace OpenTabletDriver.Web.Core.Contracts
{
    public interface IReleaseAsset
    {
        public string FileName { get; }
        public string Url { get; }
    }
}