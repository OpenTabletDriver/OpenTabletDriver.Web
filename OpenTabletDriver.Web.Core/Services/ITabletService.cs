using System.Threading.Tasks;

namespace OpenTabletDriver.Web.Core.Services
{
    public interface ITabletService
    {
        Task<string> GetMarkdownRaw();
    }
}