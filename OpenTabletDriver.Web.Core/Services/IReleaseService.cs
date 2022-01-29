using System.Collections.Generic;
using System.Threading.Tasks;
using Octokit;
using OpenTabletDriver.Web.Core.Contracts;

namespace OpenTabletDriver.Web.Core.Services
{
    public interface IReleaseService
    {
        Task<IReadOnlyList<IRelease>> GetAllReleases();
        Task<IRelease> GetLatestRelease();
        Task<IRelease> GetRelease(string tag);
    }
}