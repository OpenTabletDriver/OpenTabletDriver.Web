using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace OpenTabletDriver.Web.Core.Contracts
{
    public interface IRelease
    {
        string Name { get; }
        string Tag { get; }
        string Url { get; }
        string Body { get; }
        DateTimeOffset Date { get; }

        Task<IEnumerable<IReleaseAsset>> GetReleaseAssets();
    }
}
