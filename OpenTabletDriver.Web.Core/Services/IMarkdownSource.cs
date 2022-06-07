using System.Threading.Tasks;

namespace OpenTabletDriver.Web.Core.Services
{
    public interface IMarkdownSource
    {
        Task<string> GetPage(string category, string page);
    }
}
