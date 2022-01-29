using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using Octokit;

namespace OpenTabletDriver.Web.Core.Services
{
    public interface IRepositoryService
    {
        Task<Repository> GetRepository();
        Task<string> DownloadRepositoryTarball(Repository repository);
        Task<Stream> GetFileFromRepository(Repository repository, string relativePath);
    }
}