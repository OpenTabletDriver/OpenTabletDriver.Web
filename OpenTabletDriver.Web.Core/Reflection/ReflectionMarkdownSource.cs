using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using OpenTabletDriver.Web.Core.Services;

namespace OpenTabletDriver.Web.Core.Reflection
{
    public class ReflectionMarkdownSource : IMarkdownSource
    {
        private readonly Assembly _assembly;
        private readonly string _assemblyName;
        private readonly string[] _resources;

        public ReflectionMarkdownSource(Assembly assembly)
        {
            _assembly = assembly;
            _assemblyName = assembly.GetName().Name!;
            _resources = assembly.GetManifestResourceNames();
        }

        public async Task<string> GetPage(string category, string page)
        {
            var fileName = $"{_assemblyName}.Views.Wiki.{category}.{page}.md";
            var markdownResource = _resources.FirstOrDefault(n => n.Contains(fileName));

            if (markdownResource == null)
                return null;

            using (var stream = _assembly.GetManifestResourceStream(markdownResource)!)
            using (var sr = new StreamReader(stream))
            {
                return await sr.ReadToEndAsync();
            }
        }
    }
}
